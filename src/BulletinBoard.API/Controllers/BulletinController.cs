using System.Net;
using BulletinBoard.AppServices.Contexts.Bulletins.Services;
using BulletinBoard.Contracts.Bulletins;
using BulletinBoard.Domain.Bulletins.Entity;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoard.API.Controllers;

/// <summary>
/// Объявления.
/// </summary>
[ApiController]
[Route("[controller]")]
[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
public class BulletinController(IBulletinService bulletinService) : ControllerBase
{
    /// <summary>
    /// Создаёт объявление по модели запроса.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Идентификатор созданного объявления.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> CreateBulletinAsync([FromBody] CreateBulletinRequest request, CancellationToken cancellationToken)
    {
        var bulletinId = await bulletinService.CreateAsync(request, cancellationToken);
        
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
    /// <param name="bulletin">Модель объявления.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    [HttpPut("{bulletinId}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateBulletinAsync(BulletinDto bulletin, CancellationToken cancellationToken)
    {
        await bulletinService.UpdateAsync(bulletin, cancellationToken);
        
        return Ok();
    }

    /// <summary>
    /// Удаляет объявление по идентификатору.
    /// </summary>
    /// <param name="bulletinId">Идентификатор объявления.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    [HttpDelete("{bulletinId}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteBulletinAsync(Guid bulletinId, CancellationToken cancellationToken)
    {
        await bulletinService.DeleteAsync(bulletinId, cancellationToken);
        
        return Ok();
    }

    /// <summary>
    /// Выполняет поиск объявления по запросу.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекция объявлений.</returns>
    [HttpGet("search")]
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
}