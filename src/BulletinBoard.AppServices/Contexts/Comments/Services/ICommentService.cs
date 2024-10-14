using BulletinBoard.Contracts.Comments;

namespace BulletinBoard.AppServices.Contexts.Comments.Services;

/// <summary>
/// Сервис для работы с комментариями.
/// </summary>
public interface ICommentService
{
    /// <summary>
    /// Добавляет новый комментарий.
    /// </summary>
    /// <param name="bulletinId">Идентификатор объявления.</param>
    /// <param name="authorId">Идентификатор автора.</param>
    /// <param name="request">Запрос на добавление комментария.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Идентификатор добавленного комментария.</returns>
    Task<Guid> AddCommentAsync(Guid bulletinId, Guid authorId, AddCommentRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Удаляет комментарий по идентификатору.
    /// </summary>
    /// <param name="bulletinId">Идентификатор объявления с комментарием.</param>
    /// <param name="commentId">Идентификатор комментария.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    Task DeleteCommentAsync(Guid commentId, Guid bulletinId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Возвращает все комментарии по идентификатору объявления.
    /// </summary>
    /// <param name="bulletinId">Идентификатор объявления.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекция моделей комментариев.</returns>
    Task<ICollection<CommentDto>> GetByBulletinIdAsync(Guid bulletinId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Возвращает комментарий по идентификатору.
    /// </summary>
    /// <param name="commentId">Идентификатор.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Модель комментария.</returns>
    Task<CommentDto> GetCommentByIdAsync(Guid commentId, CancellationToken cancellationToken);
}