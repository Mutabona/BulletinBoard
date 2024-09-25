using System.Net;
using BulletinBoard.AppServices.Contexts.Files.Images.Services;
using BulletinBoard.Contracts.Files.Images;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoard.API.Controllers;

/// <summary>
/// Файлы.
/// </summary>
[ApiController]
[Route("[controller]")]
[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
public class FileController(IImageService imageService) : ControllerBase
{
    /// <summary>
    /// Добавляет изображние к объявлению по модели запроса.
    /// </summary>
    /// <param name="request">Модель запроса.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Идентификатор добавленного изображения.</returns>
    [HttpPost("upload")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UploadAsync(AddImageRequest request, CancellationToken cancellationToken)
    {
        var imageId = await imageService.AddImageAsync(request, cancellationToken);
        
        return Ok(imageId);
    }

    /// <summary>
    /// Удаляет изображние по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    [HttpDelete("delete")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await imageService.DeleteImageAsync(id, cancellationToken);
        
        return Ok();
    }

    /// <summary>
    /// Выполняет поиск изображний по идентификатору объявления.
    /// </summary>
    /// <param name="bulletinId">Идентификатор объявления.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекция моделей изображения.</returns>
    [HttpGet("bulletin/{bulletinId}")]
    [ProducesResponseType(typeof(ICollection<ImageDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetByBulletinIdAsync(Guid bulletinId, CancellationToken cancellationToken)
    {
        var images = await imageService.GetByBulletinIdAsync(bulletinId, cancellationToken);
        
        return Ok(images);
    }

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
        var image = await imageService.GetImageByIdAsync(imageId, cancellationToken);
        
        if (image == null) return NotFound();
        
        return Ok(image);
    }
}