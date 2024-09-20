using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.Infrastructure.Repository;

///<inheritdoc cref="IRepository{TEntity}"/>
public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected DbContext DbContext { get; }

    protected DbSet<TEntity> DbSet { get; }

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
    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        await DbSet.AddAsync(entity, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    ///<inheritdoc/>
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        DbSet.Remove(entity);

        await DbContext.SaveChangesAsync(cancellationToken);
    }

    ///<inheritdoc/>
    public IQueryable<TEntity> GetAll()
    {
        return DbSet.AsNoTracking();
    }

    ///<inheritdoc/>
    public ValueTask<TEntity?> GetByIdAsync(Guid id)
    {
        return DbSet.FindAsync(id);
    }

    ///<inheritdoc/>
    public IQueryable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> predicate)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));

        return DbSet.Where(predicate);
    }

    ///<inheritdoc/>
    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
        
        DbSet.Update(entity);
        await DbContext.SaveChangesAsync(cancellationToken);
    }
}