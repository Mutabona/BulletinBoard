using BulletinBoard.AppServices.Contexts.Comments.Repositories;
using BulletinBoard.Contracts.Comments;
using Microsoft.Extensions.Logging;

namespace BulletinBoard.AppServices.Contexts.Comments.Services;

///<inheritdoc cref="ICommentService"/>
public class CommentService(ICommentRepository repository, ILogger<CommentService> logger) : ICommentService
{
    /// <inheritdoc />
    public async Task<Guid> AddCommentAsync(Guid bulletinId, Guid authorId, AddCommentRequest request,
        CancellationToken cancellationToken)
    {
        logger.BeginScope("Добавление комментария по запросу: {@Request}, к объявлению {id}, пользователем: {authorId}", request, bulletinId, authorId);
        return await repository.AddCommentAsync(bulletinId, authorId, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task DeleteCommentAsync(Guid commentId, CancellationToken cancellationToken)
    {
        logger.BeginScope("Удаление комментария: {commentId}", commentId);
        await repository.DeleteCommentAsync(commentId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ICollection<CommentDto>> GetByBulletinIdAsync(Guid bulletinId, CancellationToken cancellationToken)
    {
        logger.BeginScope("Поиск комментариев по объявлению: {id}", bulletinId);
        return await repository.GetCommentsByBulletinIdAsync(bulletinId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<CommentDto> GetCommentByIdAsync(Guid commentId, CancellationToken cancellationToken)
    {
        logger.BeginScope("Поиск комментария: {id}", commentId);
        return await repository.GetCommentByIdAsync(commentId, cancellationToken);
    }
}