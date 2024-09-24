using System.Net;
using BulletinBoard.AppServices.Contexts.Bulletins.Services;
using BulletinBoard.Contracts.Bulletins;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoard.API.Controllers;

/// <summary>
/// Объявления.
/// </summary>
[ApiController]
[Route("[controller]")]
[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
public class BulletinController : ControllerBase
{
    private readonly IBulletinService _bulletinService;

    public BulletinController(IBulletinService bulletinService)
    {
        _bulletinService = bulletinService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateBulletinAsync([FromBody] CreateBulletinRequest request, CancellationToken cancellationToken)
    {
        var bulletinId = await _bulletinService.CreateAsync(request, cancellationToken);
        
        return Ok(bulletinId.ToString());
    }

    [HttpGet("{bulletinId}")]
    public async Task<IActionResult> GetBulletinByIdAsync(Guid bulletinId, CancellationToken cancellationToken)
    {
        var bulletin = await _bulletinService.FindByIdAsync(bulletinId, cancellationToken);
        
        if (bulletin == null) return NotFound();
        
        return Ok(bulletin);
    }

    [HttpPut("{bulletinId}")]
    public async Task<IActionResult> UpdateBulletinAsync(BulletinDto bulletin, CancellationToken cancellationToken)
    {
        await _bulletinService.UpdateAsync(bulletin, cancellationToken);
        
        return Ok();
    }

    [HttpDelete("{bulletinId}")]
    public async Task<IActionResult> DeleteBulletinAsync(Guid bulletinId, CancellationToken cancellationToken)
    {
        await _bulletinService.DeleteAsync(bulletinId, cancellationToken);
        
        return Ok();
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchBulletinsAsync(SearchBulletinRequest request,
        CancellationToken cancellationToken)
    {
        var bulletins = await _bulletinService.SearchBulletinsAsync(request, cancellationToken);
        
        return Ok(bulletins);
    }

    [HttpGet("by-category")]
    public async Task<IActionResult> GetByCategoryAsync(Guid bulletinCategoryId, CancellationToken cancellationToken)
    {
        var bulletins = await _bulletinService.GetByCategoryAsync(bulletinCategoryId, cancellationToken);
        
        return Ok(bulletins);
    }
}