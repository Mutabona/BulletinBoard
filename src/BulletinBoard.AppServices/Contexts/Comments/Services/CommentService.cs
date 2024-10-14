using AutoMapper;
using BulletinBoard.AppServices.Contexts.Bulletins.Services;
using BulletinBoard.AppServices.Contexts.Comments.Repositories;
using BulletinBoard.Contracts.Comments;
using BulletinBoard.Contracts.Emails;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace BulletinBoard.AppServices.Contexts.Comments.Services;

///<inheritdoc cref="ICommentService"/>
public class CommentService(ICommentRepository repository, 
    ILogger<CommentService> logger, 
    IMapper mapper, 
    IPublishEndpoint publishEndpoint, 
    TimeProvider timeProvider,
    IBulletinService bulletinService) : ICommentService
{
    /// <inheritdoc />
    public async Task<Guid> AddCommentAsync(Guid bulletinId, Guid authorId, AddCommentRequest request,
        CancellationToken cancellationToken)
    {
        await bulletinService.FindByIdAsync(bulletinId, cancellationToken);
        logger.BeginScope("Добавление комментария по запросу: {@Request}, к объявлению {id}, пользователем: {authorId}", request, bulletinId, authorId);
        
        var comment = mapper.Map<CommentDto>(request);
        comment.BulletinId = bulletinId;
        comment.AuthorId = authorId;
        comment.Id = Guid.NewGuid();
        comment.CreatedAt = timeProvider.GetUtcNow().DateTime;
        var commentId = await repository.AddCommentAsync(comment, cancellationToken);

        await publishEndpoint.Publish<CommentAdded>(new {id = commentId}, cancellationToken);
        
        return commentId;
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