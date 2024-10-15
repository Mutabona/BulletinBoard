using System.Net;
using BulletinBoard.API.Controllers.Base;
using BulletinBoard.AppServices.Contexts.Bulletins.Services;
using BulletinBoard.AppServices.Contexts.Comments.Services;
using BulletinBoard.AppServices.Contexts.Files.Images.Services;
using BulletinBoard.AppServices.Contexts.Users.Services;
using BulletinBoard.AppServices.Exceptions;
using BulletinBoard.Contracts.Bulletins;
using BulletinBoard.Contracts.Comments;
using BulletinBoard.Contracts.Files.Images;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoard.API.Controllers;

/// <summary>
/// Объявления.
/// </summary>
[ApiController]
[Route("[controller]")]
[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
public class BulletinController(
    IBulletinService bulletinService,
    ILogger<BulletinController> logger) : BaseController
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
    [ProducesResponseType(typeof(BulletinDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetBulletinByIdAsync(Guid bulletinId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Поиск объявлений с id: {id}", bulletinId);
        var bulletin = await bulletinService.FindByIdAsync(bulletinId, cancellationToken);
        
        return Ok(bulletin);
    }

    /// <summary>
    /// Изменяет объявление по модели запроса.
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
        var userId = GetCurrentUserId();
        
        logger.LogInformation("Обновление объявления с id: {id}, по запросу: {@Request}", bulletinId, request);
        await bulletinService.UpdateAsync(bulletinId, userId, request, cancellationToken);
        
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
        var userId = GetCurrentUserId();
        logger.LogInformation("Удаление объявления: {id}", bulletinId);
        await bulletinService.DeleteAsync(bulletinId, userId, cancellationToken);
        
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
    /// Выполняет поиск объявлений по идентификатору категории (включая дочерние категории).
    /// </summary>
    /// <param name="skip">Кол-во элементов для пропуска перед получением.</param>
    /// <param name="bulletinCategoryId">Идентификатор категории.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <param name="take">Кол-во элементов для получения.</param>
    /// <returns>Коллекция объявлений.</returns>
    [HttpGet("by-category")]
    [ProducesResponseType(typeof(ICollection<BulletinDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetByCategoryAsync(int take, int? skip, Guid bulletinCategoryId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Поиск объявлений по категории: {id}", bulletinCategoryId);
        var bulletins = await bulletinService.GetByCategoryAsync(take, skip, bulletinCategoryId, cancellationToken);
        
        return Ok(bulletins);
    }

    /// <summary>
    /// Возвращает все объявления.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекция объявлений.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ICollection<BulletinDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetBulletinsAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Получение всех объявлений");
        var bulletins =  await bulletinService.GetAllAsync(cancellationToken);
        
        return Ok(bulletins);
    }
}