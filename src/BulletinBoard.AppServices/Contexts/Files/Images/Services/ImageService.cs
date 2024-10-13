using AutoMapper;
using BulletinBoard.AppServices.Contexts.Files.Images.Repositories;
using BulletinBoard.Contracts.Files.Images;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BulletinBoard.AppServices.Contexts.Files.Images.Services;

///<inheritdoc cref="IImageService"/>
public class ImageService : IImageService
{
    private readonly IImageRepository _repository;
    private readonly ILogger<ImageService> _logger;
    private readonly IMapper _mapper;

    /// <summary>
    /// Создаёт экземпляр <see cref="ImageService"/>.
    /// </summary>
    /// <param name="repository">Репозиторий.</param>
    /// <param name="logger">Логгер.</param>
    /// <param name="mapper">Маппер.</param>
    public ImageService(IImageRepository repository, ILogger<ImageService> logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<Guid> AddImageAsync(Guid bulletinId, IFormFile image, CancellationToken cancellationToken)
    {
        _logger.BeginScope("Добавление изображения к объявлению: {id}", bulletinId);
        var imageEntity = _mapper.Map<ImageDto>(image);
        imageEntity.BulletinId = bulletinId;
        return await _repository.AddAsync(imageEntity, cancellationToken);
    }

    /// <inheritdoc />
    public async Task DeleteImageAsync(Guid imageId, CancellationToken cancellationToken)
    {
        _logger.BeginScope("Удаление изображения: {imageId}", imageId);
        await _repository.DeleteAsync(imageId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ICollection<Guid>> GetImageIdsByBulletinIdAsync(Guid bulletinId, CancellationToken cancellationToken)
    {
        _logger.BeginScope("Поиск изображений по объявлению: {id}", bulletinId);
        return await _repository.GetImageIdByBulletinIdAsync(bulletinId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ImageDto> GetImageByIdAsync(Guid imageId, CancellationToken cancellationToken)
    {
        _logger.BeginScope("Поиск изображения: {id}", imageId);
        return await  _repository.GetByIdAsync(imageId, cancellationToken);
    }
}