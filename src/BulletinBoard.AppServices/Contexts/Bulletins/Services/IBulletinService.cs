using BulletinBoard.Contracts.Bulletins;

namespace BulletinBoard.AppServices.Contexts.Bulletins.Services;

/// <summary>
/// Сервис для работы с объявлениями.
/// </summary>
public interface IBulletinService
{
    /// <summary>
    /// Выполняет поиск объявлений по запросу.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Коллекция объявлений.</returns>
    Task<ICollection<BulletinDto>> SearchBulletinsAsync(SearchBulletinRequest request, CancellationToken cancellationToken);
        
    /// <summary>
    /// Выполняет поиск объявлений по категории.
    /// </summary>
    /// <param name="categoryId">Идентификатор категории.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекцию кратких моделей объявлений.</returns>
    Task<ICollection<BulletinDto>> GetByCategoryAsync(Guid categoryId, CancellationToken cancellationToken);

    /// <summary>
    /// Выполняет поиск объявления по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Объявление.</returns>
    Task<BulletinDto> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Создаёт объявление.
    /// </summary>
    /// <param name="request">Запрос создания.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Идентификатор созданного объявления.</returns>
    Task<Guid> CreateAsync(CreateBulletinRequest request, CancellationToken cancellationToken);
}