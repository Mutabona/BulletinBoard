using System.Net;
using BulletinBoard.API.Controllers.Base;
using BulletinBoard.AppServices.Contexts.Files.Images.Services;
using BulletinBoard.Contracts.Files.Images;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoard.API.Controllers;

/// <summary>
/// Файлы.
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
}