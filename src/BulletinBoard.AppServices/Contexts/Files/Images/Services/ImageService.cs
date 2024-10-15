using AutoMapper;
using BulletinBoard.AppServices.Contexts.Bulletins.Services;
using BulletinBoard.AppServices.Contexts.Files.Images.Repositories;
using BulletinBoard.AppServices.Contexts.Users.Services;
using BulletinBoard.AppServices.Exceptions;
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
    private readonly TimeProvider _timeProvider;
    private readonly IBulletinService _bulletinService;

    /// <summary>
    /// Создаёт экземпляр <see cref="ImageService"/>.
    /// </summary>
    /// <param name="repository">Репозиторий.</param>
    /// <param name="logger">Логгер.</param>
    /// <param name="mapper">Маппер.</param>
    /// <param name="timeProvider">Провайдер для работы со временем.</param>
    /// <param name="bulletinService">Сервис для работы с объявлениями.</param>
    public ImageService(IImageRepository repository, ILogger<ImageService> logger, IMapper mapper, TimeProvider timeProvider, IBulletinService bulletinService)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
        _timeProvider = timeProvider;
        _bulletinService = bulletinService;
    }

    /// <inheritdoc />
    public async Task<Guid> AddImageAsync(Guid bulletinId, Guid userId, IFormFile image, CancellationToken cancellationToken)
    {
        var isUserOwner = await _bulletinService.IsUserBulletinsOwnerAsync(bulletinId, userId, cancellationToken);
        if (!isUserOwner) throw new ForbiddenException();
        
        _logger.BeginScope("Добавление изображения к объявлению: {id}", bulletinId);
        var imageEntity = _mapper.Map<ImageDto>(image);
        imageEntity.BulletinId = bulletinId;
        imageEntity.Id = Guid.NewGuid();
        imageEntity.CreatedAt = _timeProvider.GetUtcNow().UtcDateTime;
        return await _repository.AddAsync(imageEntity, cancellationToken);
    }

    /// <inheritdoc />
    public async Task DeleteImageAsync(Guid imageId, Guid userId, CancellationToken cancellationToken)
    {
        var image = await _repository.GetByIdAsync(imageId, cancellationToken);
        var bulletin = await _bulletinService.FindByIdAsync(image.BulletinId, cancellationToken);
        
        var isUserOwner = await _bulletinService.IsUserBulletinsOwnerAsync(bulletin.Id, userId, cancellationToken);
        if (!isUserOwner) throw new ForbiddenException();
        
        _logger.BeginScope("Удаление изображения: {imageId}", imageId);
        await _repository.DeleteAsync(imageId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ICollection<Guid>> GetImageIdsByBulletinIdAsync(Guid bulletinId, CancellationToken cancellationToken)
    {
        await _bulletinService.FindByIdAsync(bulletinId, cancellationToken);
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