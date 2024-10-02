using BulletinBoard.AppServices.Contexts.Comments.Repositories;
using BulletinBoard.Contracts.Comments;

namespace BulletinBoard.AppServices.Contexts.Comments.Services;

///<inheritdoc cref="ICommentService"/>
public class CommentService(ICommentRepository repository) : ICommentService
{
    /// <inheritdoc />
    public async Task<Guid> AddCommentAsync(Guid bulletinId, Guid authorId, AddCommentRequest request,
        CancellationToken cancellationToken)
    {
        return await repository.AddCommentAsync(bulletinId, authorId, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task DeleteCommentAsync(Guid commentId, CancellationToken cancellationToken)
    {
        await repository.DeleteCommentAsync(commentId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ICollection<CommentDto>> GetByBulletinIdAsync(Guid bulletinId, CancellationToken cancellationToken)
    {
        return await repository.GetCommentsByBulletinIdAsync(bulletinId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<CommentDto> GetCommentByIdAsync(Guid commentId, CancellationToken cancellationToken)
    {
        return await repository.GetCommentByIdAsync(commentId, cancellationToken);
    }
}