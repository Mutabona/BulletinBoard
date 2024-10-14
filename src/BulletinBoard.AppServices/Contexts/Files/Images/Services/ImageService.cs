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
    private readonly IUserService _userService;

    /// <summary>
    /// Создаёт экземпляр <see cref="ImageService"/>.
    /// </summary>
    /// <param name="repository">Репозиторий.</param>
    /// <param name="logger">Логгер.</param>
    /// <param name="mapper">Маппер.</param>
    /// <param name="timeProvider">Провайдер для работы со временем.</param>
    /// <param name="bulletinService">Сервис для работы с объявлениями.</param>
    /// <param name="userService">Сервис для работы с пользователями.</param>
    public ImageService(IImageRepository repository, ILogger<ImageService> logger, IMapper mapper, TimeProvider timeProvider, IBulletinService bulletinService, IUserService userService)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
        _timeProvider = timeProvider;
        _bulletinService = bulletinService;
        _userService = userService;
    }

    /// <inheritdoc />
    public async Task<Guid> AddImageAsync(Guid bulletinId, Guid userId, IFormFile image, CancellationToken cancellationToken)
    {
        var bulletin = await _bulletinService.FindByIdAsync(bulletinId, cancellationToken);
        var user = await _userService.GetUserByIdAsync(userId, cancellationToken);
        if (bulletin.OwnerId != user.Id) throw new ForbiddenException();
        
        _logger.BeginScope("Добавление изображения к объявлению: {id}", bulletinId);
        var imageEntity = _mapper.Map<ImageDto>(image);
        imageEntity.BulletinId = bulletinId;
        imageEntity.Id = Guid.NewGuid();
        imageEntity.CreatedAt = _timeProvider.GetUtcNow().DateTime;
        return await _repository.AddAsync(imageEntity, cancellationToken);
    }

    /// <inheritdoc />
    public async Task DeleteImageAsync(Guid bulletinId, Guid imageId, Guid userId, CancellationToken cancellationToken)
    {
        var bulletin = await _bulletinService.FindByIdAsync(bulletinId, cancellationToken);
        var user = await _userService.GetUserByIdAsync(userId, cancellationToken);
        if (bulletin.OwnerId != user.Id) throw new ForbiddenException();
        
        var image = await _repository.GetByIdAsync(imageId, cancellationToken);
        if (image.BulletinId != bulletinId) throw new ConflictException();
        
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