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
    /// <param name="take">Количество элементов для получения.</param>
    /// <param name="skip">Количество пропускаемых элементов.</param>
    /// <param name="categoryId">Идентификатор категории.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекцию кратких моделей объявлений.</returns>
    Task<ICollection<BulletinDto>> GetByCategoryAsync(int take, int? skip, Guid categoryId, CancellationToken cancellationToken);

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
    /// <param name="ownerId">Идентификатор владельца.</param>
    /// <param name="request">Запрос создания.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Идентификатор созданного объявления.</returns>
    Task<Guid> CreateAsync(Guid ownerId, CreateBulletinRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Обновляет объявление.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя отправившего запрос.</param>
    /// <param name="request">Запрос на обновление объявления.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <param name="bulletinId">Идентификатор объявления.</param>
    /// <returns></returns>
    Task UpdateAsync(Guid bulletinId, Guid userId, UpdateBulletinRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Удаляет объявление по идентификатору.
    /// </summary>
    /// <param name="bulletinId">Идентификатор объявления.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    Task DeleteAsync(Guid bulletinId, Guid userId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Получает все объявления.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекция моделей объявлений.</returns>
    Task<ICollection<BulletinDto>> GetAllAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Проверяет является ли пользователь владельцем объявления.
    /// </summary>
    /// <param name="bulletinId">Идентификатор объявления.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>True, если является, false, если не является.</returns>
    Task<bool> IsUserBulletinsOwnerAsync(Guid bulletinId, Guid userId, CancellationToken cancellationToken);
}