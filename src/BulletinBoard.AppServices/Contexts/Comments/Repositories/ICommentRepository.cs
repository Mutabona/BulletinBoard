using BulletinBoard.Contracts.Comments;

namespace BulletinBoard.AppServices.Contexts.Comments.Repositories;

/// <summary>
/// Репозиторий для работы с комментариями.
/// </summary>
public interface ICommentRepository
{
    /// <summary>
    /// Сохраняет комментарий.
    /// </summary>
    /// <param name="comment">Комментарий./</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Идентификатор созданного комментария.</returns>
    Task<Guid> AddCommentAsync(CommentDto comment, CancellationToken cancellationToken);
    
    /// <summary>
    /// Удаляет комментарий.
    /// </summary>
    /// <param name="commentId">Идентификатор комментария.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    Task DeleteCommentAsync(Guid commentId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Возвращает все комментарии по идентификатору объявления.
    /// </summary>
    /// <param name="bulletinId">Идентификатор объявления.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    Task<ICollection<CommentDto>> GetCommentsByBulletinIdAsync(Guid bulletinId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Возвращает комментарий по идентификатору.
    /// </summary>
    /// <param name="commentId">Идентификатор.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Модель комментария.</returns>
    Task<CommentDto> GetCommentByIdAsync(Guid commentId, CancellationToken cancellationToken);
}