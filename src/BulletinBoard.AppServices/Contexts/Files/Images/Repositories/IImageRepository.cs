using BulletinBoard.Contracts.Files.Images;

namespace BulletinBoard.AppServices.Contexts.Files.Images.Repositories;

/// <summary>
/// Репозиторий дя работы с изображениями.
/// </summary>
public interface IImageRepository
{
    /// <summary>
    /// Сохраняет изображение.
    /// </summary>
    /// <param name="image">Изображение.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Идентификатор изображения.</returns>
    Task<Guid> AddAsync(AddImageRequest image, CancellationToken cancellationToken);
    
    /// <summary>
    /// Удаляет изображение по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    
    /// <summary>
    /// Возвращает изображения объявления.
    /// </summary>
    /// <param name="bulletinId">Идентификатор объявления.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекция изображений.</returns>
    Task<ICollection<ImageDto>> GetByBulletinIdAsync(Guid bulletinId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Возвращает изображение по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Изображение.</returns>
    Task<ImageDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}