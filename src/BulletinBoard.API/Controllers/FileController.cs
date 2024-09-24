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
public class FileController : ControllerBase
{
    public readonly IImageService _imageService;

    public FileController(IImageService imageService)
    {
        _imageService = imageService;
    }
    
    [HttpPost("upload")]
    public async Task<IActionResult> UploadAsync(AddImageRequest request, CancellationToken cancellationToken)
    {
        var imageId = await _imageService.AddImageAsync(request, cancellationToken);
        
        return Ok(imageId);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _imageService.DeleteImageAsync(id, cancellationToken);
        
        return Ok();
    }

    [HttpGet("bulletin/{bulletinId}")]
    public async Task<IActionResult> GetByBulletinIdAsync(Guid bulletinId, CancellationToken cancellationToken)
    {
        var images = await _imageService.GetByBulletinIdAsync(bulletinId, cancellationToken);
        
        return Ok(images);
    }

    [HttpGet("{imageId}")]
    public async Task<IActionResult> GetByImageIdAsync(Guid imageId, CancellationToken cancellationToken)
    {
        var image = await _imageService.GetImageByIdAsync(imageId, cancellationToken);
        
        if (image == null) return NotFound();
        
        return Ok(image);
    }
}