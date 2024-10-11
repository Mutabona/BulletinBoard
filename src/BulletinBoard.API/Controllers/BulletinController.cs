using System.Net;
using BulletinBoard.API.Controllers.Base;
using BulletinBoard.AppServices.Contexts.Bulletins.Services;
using BulletinBoard.AppServices.Contexts.Comments.Services;
using BulletinBoard.AppServices.Contexts.Files.Images.Services;
using BulletinBoard.AppServices.Exceptions;
using BulletinBoard.Contracts.Bulletins;
using BulletinBoard.Contracts.Comments;
using BulletinBoard.Contracts.Files.Images;
using BulletinBoard.Domain.Bulletins.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoard.API.Controllers;

/// <summary>
/// Объявления.
/// </summary>
[ApiController]
[Route("[controller]")]
[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
public class BulletinController(IBulletinService bulletinService, IImageService imageService, ICommentService commentService, ILogger<BulletinController> logger) : BaseController
{
    /// <summary>
    /// Создаёт объявление по модели запроса.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Идентификатор созданного объявления.</returns>
    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> CreateBulletinAsync([FromBody] CreateBulletinRequest request, CancellationToken cancellationToken)
    {
        var ownerId = GetCurrentUserId();
        logger.LogInformation("Создание объявления по запросу: {@Request}, пользователем с id: {id}", request, ownerId);
        var bulletinId = await bulletinService.CreateAsync(ownerId, request, cancellationToken);
        
        return StatusCode((int)HttpStatusCode.Created, bulletinId.ToString());
    }

    /// <summary>
    /// Выполняет поиск объявления по идентификатору.
    /// </summary>
    /// <param name="bulletinId">Идентификатор объявления.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Модель объявления.</returns>
    [HttpGet("{bulletinId}")]
    [ProducesResponseType(typeof(Bulletin), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetBulletinByIdAsync(Guid bulletinId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Поиск объявлений с id: {id}", bulletinId);
        var bulletin = await bulletinService.FindByIdAsync(bulletinId, cancellationToken);
        
        return Ok(bulletin);
    }

    /// <summary>
    /// Изменяет объявление по модели.
    /// </summary>
    /// <param name="request">Запрос на обновление объявления.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <param name="bulletinId">Идентификатор объявления.</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut("{bulletinId}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> UpdateBulletinAsync(Guid bulletinId, UpdateBulletinRequest request, CancellationToken cancellationToken)
    {
        BulletinDto bulletin;
        try
        {
            bulletin = await bulletinService.FindByIdAsync(bulletinId, cancellationToken);
        }
        catch (EntityNotFoundException)
        {
            return NotFound("Объявление не найдено.");
        }
        
        var userId = GetCurrentUserId();
        
        if (bulletin.OwnerId != userId) return Forbid();
        
        logger.LogInformation("Обновление объявления с id: {id}, по запросу: {@Request}", bulletinId, request);
        await bulletinService.UpdateAsync(bulletinId, request, cancellationToken);
        
        return Ok();
    }

    /// <summary>
    /// Удаляет объявление по идентификатору.
    /// </summary>
    /// <param name="bulletinId">Идентификатор объявления.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete("{bulletinId}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> DeleteBulletinAsync(Guid bulletinId, CancellationToken cancellationToken)
    {
        BulletinDto bulletin;
        try
        {
            bulletin = await bulletinService.FindByIdAsync(bulletinId, cancellationToken);
        }
        catch (EntityNotFoundException)
        {
            return NotFound("Объявление не найдено.");
        }
        
        if (!(bulletin.OwnerId == GetCurrentUserId() || GetCurrentUserRole() == "Admin")) return Forbid();
        
        logger.LogInformation("Удаление объявления: {id}", bulletinId);
        await bulletinService.DeleteAsync(bulletinId, cancellationToken);
        
        return NoContent();
    }

    /// <summary>
    /// Выполняет поиск объявления по запросу.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекция объявлений.</returns>
    [HttpPost("search")]
    [ProducesResponseType(typeof(ICollection<BulletinDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> SearchBulletinsAsync(SearchBulletinRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Поиск объявлений по запросу: {@Request}", request);
        var bulletins = await bulletinService.SearchBulletinsAsync(request, cancellationToken);
        
        return Ok(bulletins);
    }
    
    /// <summary>
    /// Выполняет поиск объявлений по идентификатору категории (включая дочерние категории)
    /// </summary>
    /// <param name="bulletinCategoryId">Идентификатор категории.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекция объявлений.</returns>
    [HttpGet("by-category")]
    [ProducesResponseType(typeof(ICollection<BulletinDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetByCategoryAsync(Guid bulletinCategoryId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Поиск объявлений по категории: {id}", bulletinCategoryId);
        var bulletins = await bulletinService.GetByCategoryAsync(bulletinCategoryId, cancellationToken);
        
        return Ok(bulletins);
    }

    /// <summary>
    /// Возвращает все объявления.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекция объявлений.</returns>
    [HttpGet("all")]
    [ProducesResponseType(typeof(ICollection<BulletinDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetBulletinsAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Получение всех объявлений");
        var bulletins =  await bulletinService.GetAllAsync(cancellationToken);
        
        return Ok(bulletins);
    }

    /// <summary>
    /// Добавляет изображение к объявлению.
    /// </summary>
    /// <param name="bulletinId">Идентификатор объявления.</param>
    /// <param name="file">Изображение.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Идентификатор добавленного изображения.</returns>
    [Authorize]
    [HttpPost("{bulletinId}/images/upload")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> UploadImageAsync(Guid bulletinId, IFormFile file, CancellationToken cancellationToken)
    {
        BulletinDto bulletin;
        try
        {
            bulletin = await bulletinService.FindByIdAsync(bulletinId, cancellationToken);
        }
        catch (EntityNotFoundException)
        {
            return NotFound("Объявление не найдено.");
        }
        
        var userId = GetCurrentUserId();
        
        if (userId != bulletin.OwnerId) return Forbid();
        
        logger.LogInformation("Добавление изображения к объявлению: {id}", bulletinId);
        var fileId = await imageService.AddImageAsync(bulletinId, file, cancellationToken);
        return StatusCode((int)HttpStatusCode.Created, fileId.ToString());
    }

    /// <summary>
    /// Возвращает идентификаторы всех изображений по идентификатору объявления.
    /// </summary>
    /// <param name="bulletinId">Идентификатор объявления.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекция идентификаторов изображений.</returns>
    [HttpGet("{bulletinId}/images")]
    [ProducesResponseType(typeof(ICollection<Guid>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetImagesAsync(Guid bulletinId, CancellationToken cancellationToken)
    {
        try
        {
            await bulletinService.FindByIdAsync(bulletinId, cancellationToken);
        }
        catch (EntityNotFoundException)
        {
            return NotFound("Объявление не найдено.");
        }
        logger.LogInformation("Поиск изображений по объявлению: {id}", bulletinId);
        var imageIds = await imageService.GetImageIdsByBulletinIdAsync(bulletinId, cancellationToken);
        return Ok(imageIds);
    }

    /// <summary>
    /// Удаляет изображение из объявления по идентификатору.
    /// </summary>
    /// <param name="imageId">Идентификатор изображения.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <param name="bulletinId">Идентификатор объявления с изображением.</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete("{bulletinId}/images/{imageId}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> DeleteAsync(Guid bulletinId, Guid imageId, CancellationToken cancellationToken)
    {
        BulletinDto bulletin;
        try
        {
            bulletin = await bulletinService.FindByIdAsync(bulletinId, cancellationToken);
        }
        catch (EntityNotFoundException)
        {
            return NotFound("Объявление не найдено.");
        }
        
        var userId = GetCurrentUserId();
        if (userId != bulletin.OwnerId) return Forbid();

        ImageDto image;
        try
        {
            image = await imageService.GetImageByIdAsync(imageId, cancellationToken);
        }
        catch (EntityNotFoundException)
        {
            return NotFound("Изображение не найдено.");
        }
        
        if (bulletin.Id != image.BulletinId) return BadRequest("Изображение не принадлежит объявлению.");
        
        logger.LogInformation("Удаление изображения: {imageId}, объявления: {bulletinId}", imageId, bulletinId);
        await imageService.DeleteImageAsync(imageId, cancellationToken);
        
        return NoContent();
    }

    /// <summary>
    /// Добавляет комментарий к объявлению.
    /// </summary>
    /// <param name="bulletinId">Идентификатор объявления.</param>
    /// <param name="comment">Комментарий.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Идентификатор комментария.</returns>
    [Authorize]
    [HttpPost("{bulletinId}/comments")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ICollection<Guid>), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> AddCommentAsync(Guid bulletinId, AddCommentRequest comment, CancellationToken cancellationToken)
    {
        try
        {
            await bulletinService.FindByIdAsync(bulletinId, cancellationToken);
        }
        catch (EntityNotFoundException)
        {
            return NotFound("Объявление не найдено.");
        }
        
        var authorId = GetCurrentUserId();
        
        logger.LogInformation("Добавление комментария по запросу: {@Request}, к объявлению {id}, пользователем: {authorId}", comment, bulletinId, authorId);
        var commentId = await commentService.AddCommentAsync(bulletinId, authorId, comment, cancellationToken);
        return StatusCode((int)HttpStatusCode.Created, commentId.ToString());
    }

    /// <summary>
    /// Удаляет комментарий объявления.
    /// </summary>
    /// <param name="bulletinId">Идентификатор объявления.</param>
    /// <param name="commentId">Идентификатор комментария.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{bulletinId}/comments/{commentId}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> DeleteCommentAsync(Guid bulletinId, Guid commentId, CancellationToken cancellationToken)
    {
        try
        {
            await bulletinService.FindByIdAsync(bulletinId, cancellationToken);
        }
        catch (EntityNotFoundException)
        {
            return NotFound("Объявление не найдено.");
        }

        CommentDto comment;
        try
        {
            comment = await commentService.GetCommentByIdAsync(commentId, cancellationToken);
        }
        catch (EntityNotFoundException)
        {
            return NotFound("Комментарий не найден.");
        }
        
        if (bulletinId != comment.BulletinId) return BadRequest();
        
        logger.LogInformation("Удаление комментария: {commentId}, у объявления: {bulletinId}", commentId, bulletinId);
        await commentService.DeleteCommentAsync(commentId, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Получает все комментарии по идентификатору объявления.
    /// </summary>
    /// <param name="bulletinId">Идентификатор объявления.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекция моделей комментариев.</returns>
    [HttpGet("{bulletinId}/comments")]
    [ProducesResponseType(typeof(ICollection<CommentDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetBulletinsCommentsAsync(Guid bulletinId, CancellationToken cancellationToken)
    {
        try
        {
            await bulletinService.FindByIdAsync(bulletinId, cancellationToken);
        }
        catch (EntityNotFoundException)
        {
            return NotFound("Объявление не найдено.");
        }
        
        logger.LogInformation("Поиск комментариев по объявлению: {id}", bulletinId);
        var comments = await commentService.GetByBulletinIdAsync(bulletinId, cancellationToken);
        
        return Ok(comments);
    }
}