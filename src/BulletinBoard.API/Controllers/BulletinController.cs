using System.Net;
using System.Security.Claims;
using BulletinBoard.API.Controllers.Base;
using BulletinBoard.AppServices.Contexts.Bulletins.Services;
using BulletinBoard.AppServices.Contexts.Comments.Services;
using BulletinBoard.AppServices.Contexts.Files.Images.Services;
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
public class BulletinController(IBulletinService bulletinService, IImageService imageService, ICommentService commentService) : BaseController
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
        var ownerId = GetCurrentUserIdAsync();
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
        var bulletin = await bulletinService.FindByIdAsync(bulletinId, cancellationToken);
        
        if (bulletin == null) return NotFound();
        
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
        var bulletin = await bulletinService.FindByIdAsync(bulletinId, cancellationToken);
        if (bulletin == null) return NotFound();
        
        var userId = GetCurrentUserIdAsync();
        
        if (bulletin.OwnerId != userId) return Forbid();
        
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
        var bulletin = await bulletinService.FindByIdAsync(bulletinId, cancellationToken);
        
        if (bulletin == null) return NotFound();

        var userId = GetCurrentUserIdAsync();
        
        if (!(bulletin.OwnerId.Equals(userId) || HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Role).Value == "Admin")) return Forbid();
        
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
        var bulletin = await bulletinService.FindByIdAsync(bulletinId, cancellationToken);
        if (bulletin == null) return NotFound("Объявление не существует.");
        
        var userId = GetCurrentUserIdAsync();
        
        if (userId != bulletin.OwnerId) return Forbid();
        
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
        var bulletin = await bulletinService.FindByIdAsync(bulletinId, cancellationToken);
        if (bulletin == null) return NotFound("Объявление не существует.");
        
        var imageIds = await imageService.GetImageIdsByBulletinIdAsync(bulletinId, cancellationToken);
        return Ok(imageIds);
    }

    /// <summary>
    /// Удаляет изображние из объявления по идентификатору.
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
        var bulletin = await bulletinService.FindByIdAsync(bulletinId, cancellationToken);
        if (bulletin == null) return NotFound("Объявление не существует.");
        
        var userId = GetCurrentUserIdAsync();
        if (userId != bulletin.OwnerId) return Forbid();
        
        var image = await imageService.GetImageByIdAsync(imageId, cancellationToken);
        if (bulletin.Id != image.BulletinId) return BadRequest("Изображение не принадлежит объявлению.");
        
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
        var bulletin = await bulletinService.FindByIdAsync(bulletinId, cancellationToken);
        if (bulletin == null) return NotFound("Объявление не существует.");
        
        var authorId = GetCurrentUserIdAsync();
        
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
    public async Task<IActionResult> DeleteCommentAsync(Guid bulletinId, Guid commentId, CancellationToken cancellationToken)
    {
        var comment = await commentService.GetCommentByIdAsync(commentId, cancellationToken);
        if (bulletinId != comment.BulletinId) return BadRequest();
        
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
        var bulletin = await bulletinService.FindByIdAsync(bulletinId, cancellationToken);
        if (bulletin == null) return NotFound("Объявление не существует.");
        
        
        var comments = await commentService.GetByBulletinIdAsync(bulletinId, cancellationToken);
        
        return Ok(comments);
    }
}