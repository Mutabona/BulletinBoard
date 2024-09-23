using BulletinBoard.Contracts.Files.Images;

namespace BulletinBoard.AppServices.Contexts.Files.Images.Services;

/// <summary>
/// Сервис для работы с изображениями.
/// </summary>
public interface IImageService
{
    /// <summary>
    /// Добавляет новое изображение.
    /// </summary>
    /// <param name="request">Изображение.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Идентификатор добавленного изображения</returns>
    Task<Guid> AddImageAsync(AddImageRequest request, CancellationToken cancellationToken);
    
    /// <summary>
    /// Удаляет изображение.
    /// </summary>
    /// <param name="imageId">Идентификатор изображения.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    Task DeleteImageAsync(Guid imageId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Возвращает изображения по объявлению.
    /// </summary>
    /// <param name="bulletinId">Идентификатор объявления.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекция изображений.</returns>
    Task<ICollection<ImageDto>> GetByBulletinIdAsync(Guid bulletinId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Возвращает изображение по идентификатору.
    /// </summary>
    /// <param name="imageId">Идентификатор изображения.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Изображение.</returns>
    Task<ImageDto> GetImageByIdAsync(Guid imageId, CancellationToken cancellationToken);
}