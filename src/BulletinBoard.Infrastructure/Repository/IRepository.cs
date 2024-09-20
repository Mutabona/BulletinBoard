using System.Linq.Expressions;

namespace BulletinBoard.Infrastructure.Repository;

/// <summary>
    /// Репозиторий.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Возвращает все элементы сущности <see cref="TEntity"/>
        /// </summary>
        /// <returns>Все элементы сущности <see cref="TEntity"/></returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// Возвращает все элементы сущности <see cref="TEntity"/> по предикату
        /// </summary>
        /// <param name="predicate">Предикат</param>
        /// <returns>Все элементы сущности <see cref="TEntity"/> по предикату</returns>
        IQueryable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Возвращает элемент сущности <see cref="TEntity"/> по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор сущности</param>
        /// <returns>Элемент сущности <see cref="TEntity"/> по идентификатору</returns>
        ValueTask<TEntity> GetByIdAsync(Guid id);

        /// <summary>
        /// Добавление элемента в репозиторий.
        /// </summary>
        /// <param name="entity">Сущность <see cref="TEntity"/></param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns></returns>
        Task AddAsync(TEntity entity, CancellationToken cancellationToken);

        /// <summary>
        /// Обновление сущности в репозитории.
        /// </summary>
        /// <param name="entity">Сущность <see cref="TEntity"/></param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns></returns>
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);

        /// <summary>
        /// Удаление сущности из репозитория.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns></returns>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }