using System.Linq.Expressions;
using BulletinBoard.AppServices.Exceptions;
using BulletinBoard.Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.Infrastructure.Repository;

///<inheritdoc cref="IRepository{TEntity}"/>
public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    private DbContext DbContext { get; }
    private DbSet<TEntity> DbSet { get; }

    /// <summary>
    /// Создаёт экземпляр <see cref="Repository"/>.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных <see cref="DbContext"/></param>
    public Repository(DbContext dbContext)
    {
        DbContext = dbContext;
        DbSet = DbContext.Set<TEntity>();
    }

    ///<inheritdoc/>
    public async Task<Guid> AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        await DbSet.AddAsync(entity, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    ///<inheritdoc/>
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity == null) throw new EntityNotFoundException();
        
        DbSet.Remove(entity);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    ///<inheritdoc/>
    public IQueryable<TEntity> GetAll()
    {
        return DbSet.AsNoTracking();
    }

    ///<inheritdoc/>
    public async Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await DbSet.FindAsync(id, cancellationToken);
        if (entity == null) throw new EntityNotFoundException();
        return entity;
    }

    ///<inheritdoc/>
    public IQueryable<TEntity> GetByPredicate(Expression<Func<TEntity, bool>> predicate)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));

        return DbSet.Where(predicate);
    }

    ///<inheritdoc/>
    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        if (entity == null)
        {
            throw new EntityNotFoundException();
        }
        
        DbSet.Update(entity);
        await DbContext.SaveChangesAsync(cancellationToken);
    }
}