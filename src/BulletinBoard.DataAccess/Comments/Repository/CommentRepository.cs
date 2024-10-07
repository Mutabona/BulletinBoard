using AutoMapper;
using AutoMapper.QueryableExtensions;
using BulletinBoard.AppServices.Contexts.Comments.Repositories;
using BulletinBoard.AppServices.Exceptions;
using BulletinBoard.Contracts.Comments;
using BulletinBoard.Domain.Comments.Entity;
using BulletinBoard.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.DataAccess.Comments.Repository;

///<inheritdoc cref="ICommentRepository"/>
public class CommentRepository(IRepository<Comment> repository, IMapper mapper) : ICommentRepository
{
    ///<inheritdoc/>
    public async Task<Guid> AddCommentAsync(Guid bulletinId, Guid authorId, AddCommentRequest comment, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<Comment>(comment);
        entity.AuthorId = authorId;
        entity.BulletinId = bulletinId;
        
        return await repository.AddAsync(entity, cancellationToken);
    }

    ///<inheritdoc/>
    public async Task DeleteCommentAsync(Guid commentId, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(commentId, cancellationToken);
    }

    ///<inheritdoc/>
    public async Task<ICollection<CommentDto>> GetCommentsByBulletinIdAsync(Guid bulletinId, CancellationToken cancellationToken)
    {
        return await repository.GetAll().Where(c => c.BulletinId == bulletinId).Include(x => x.Author).ProjectTo<CommentDto>(mapper.ConfigurationProvider).ToListAsync(cancellationToken);
    }

    ///<inheritdoc/>
    public async Task<CommentDto> GetCommentByIdAsync(Guid commentId, CancellationToken cancellationToken)
    {
        var comment =  await repository.GetAll().Where(c => c.Id == commentId).Include(x => x.Author).FirstOrDefaultAsync(cancellationToken);
        if (comment == null) throw new EntityNotFoundException();
        return mapper.Map<CommentDto>(comment);
    }
}