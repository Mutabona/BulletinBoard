using BulletinBoard.AppServices.Specifications;
using BulletinBoard.Contracts.Bulletins;
using BulletinBoard.Domain.Bulletins.Entity;

namespace BulletinBoard.AppServices.Contexts.Bulletins.Repositories;

/// <summary>
/// Репозиторий для работы с объявлениями.
/// </summary>
public interface IBulletinRepository
{
    /// <summary>
    /// Выполняет получение объявлений по спецификации с пагинацией.
    /// </summary>
    /// <param name="specification">Спецификация.</param>
    /// <param name="take">Количество элементов для выборки.</param>
    /// <param name="skip">Количество элементов для пропуска.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекция моделей объявлений.</returns>
    Task<ICollection<BulletinDto>> GetBySpecificationWithPaginationAsync(
        ISpecification<Bulletin> specification, 
        int take, 
        int? skip,
        CancellationToken cancellationToken);
        
    /// <summary>
    /// Выполняет получение объявлений по спецификации.
    /// </summary>
    /// <param name="specification">Спецификация.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекция моделей объявлений.</returns>
    Task<ICollection<BulletinDto>> GetBySpecificationAsync(
        ISpecification<Bulletin> specification,
        CancellationToken cancellationToken);
        
    /// <summary>
    /// Получает объявление по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Модель объявления.</returns>
    Task<BulletinDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Создаёт объявление по модели запроса.
    /// </summary>
    /// <param name="bulletin">Объявление.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Идентификатор созданного объявления.</returns>
    Task<Guid> CreateAsync(BulletinDto bulletin, CancellationToken cancellationToken);

    /// <summary>
    /// Обновляет объявление.
    /// </summary>
    /// <param name="bulletin">Объявление.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    Task UpdateAsync(BulletinDto bulletin, CancellationToken cancellationToken);
    
    /// <summary>
    /// Удаляет объявление по идентификатору..
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    
    /// <summary>
    /// Получает все объявления.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекция объявлений.</returns>
    Task<ICollection<BulletinDto>> GetAllAsync(CancellationToken cancellationToken);
}