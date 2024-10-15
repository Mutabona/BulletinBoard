using System.Net;
using BulletinBoard.API.Controllers.Base;
using BulletinBoard.AppServices.Contexts.Files.Images.Services;
using BulletinBoard.Contracts.Files.Images;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoard.API.Controllers;

/// <summary>
/// Изображения.
/// </summary>
[ApiController]
[Route("[controller]")]
[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
public class ImageController(IImageService imageService, ILogger<ImageController> logger) : BaseController
{
    /// <summary>
    /// Выполняет поиск изображения по идентификатору.
    /// </summary>
    /// <param name="imageId">Идентификатор изображения.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Модель изображения.</returns>
    [HttpGet("{imageId}")]
    [ProducesResponseType(typeof(ImageDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetByImageIdAsync(Guid imageId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Поиск изображения: {id}", imageId);
        var image = await imageService.GetImageByIdAsync(imageId, cancellationToken);
        
        return File(image.Content, image.ContentType);
    }
    
    /// <summary>
    /// Добавляет изображение к объявлению.
    /// </summary>
    /// <param name="bulletinId">Идентификатор объявления.</param>
    /// <param name="file">Изображение.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Идентификатор добавленного изображения.</returns>
    [Authorize]
    [HttpPost("/Bulletin/{bulletinId}/Image")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> UploadImageAsync(Guid bulletinId, IFormFile file, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        logger.LogInformation("Добавление изображения к объявлению: {id}", bulletinId);
        var fileId = await imageService.AddImageAsync(bulletinId, userId, file, cancellationToken);
        return StatusCode((int)HttpStatusCode.Created, fileId.ToString());
    }

    /// <summary>
    /// Возвращает идентификаторы всех изображений по идентификатору объявления.
    /// </summary>
    /// <param name="bulletinId">Идентификатор объявления.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекция идентификаторов изображений.</returns>
    [HttpGet("/Bulletin/{bulletinId}/Image/ids")]
    [ProducesResponseType(typeof(ICollection<Guid>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetImagesAsync(Guid bulletinId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Поиск изображений по объявлению: {id}", bulletinId);
        var imageIds = await imageService.GetImageIdsByBulletinIdAsync(bulletinId, cancellationToken);
        return Ok(imageIds);
    }

    /// <summary>
    /// Удаляет изображение по идентификатору.
    /// </summary>
    /// <param name="imageId">Идентификатор изображения.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete("{imageId}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> DeleteAsync(Guid imageId, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        logger.LogInformation("Удаление изображения: {imageId}", imageId);
        await imageService.DeleteImageAsync(imageId, userId, cancellationToken);
        
        return NoContent();
    }
}