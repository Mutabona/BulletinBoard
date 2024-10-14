using System.Net;
using BulletinBoard.API.Controllers.Base;
using BulletinBoard.AppServices.Contexts.Comments.Services;
using BulletinBoard.AppServices.Exceptions;
using BulletinBoard.Contracts.Bulletins;
using BulletinBoard.Contracts.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoard.API.Controllers;

/// <summary>
/// Комментарии.
/// </summary>
[ApiController]
[Route("[controller]")]
[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
public class CommentController(ICommentService commentService, ILogger<CommentController> logger) : BaseController
{
    /// <summary>
    /// Добавляет комментарий к объявлению.
    /// </summary>
    /// <param name="bulletinId">Идентификатор объявления.</param>
    /// <param name="comment">Комментарий.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Идентификатор комментария.</returns>
    [Authorize]
    [HttpPost("{bulletinId}")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ICollection<Guid>), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> AddCommentAsync(Guid bulletinId, AddCommentRequest comment, CancellationToken cancellationToken)
    {
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
    [HttpDelete("{bulletinId}/{commentId}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> DeleteCommentAsync(Guid bulletinId, Guid commentId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Удаление комментария: {commentId}, у объявления: {bulletinId}", commentId, bulletinId);
        await commentService.DeleteCommentAsync(commentId, bulletinId, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Получает все комментарии по идентификатору объявления.
    /// </summary>
    /// <param name="bulletinId">Идентификатор объявления.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекция моделей комментариев.</returns>
    [HttpGet("{bulletinId}")]
    [ProducesResponseType(typeof(ICollection<CommentDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetBulletinsCommentsAsync(Guid bulletinId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Поиск комментариев по объявлению: {id}", bulletinId);
        var comments = await commentService.GetByBulletinIdAsync(bulletinId, cancellationToken);
        
        return Ok(comments);
    }
}