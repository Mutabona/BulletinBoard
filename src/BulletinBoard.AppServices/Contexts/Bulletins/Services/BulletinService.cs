using AutoMapper;
using BulletinBoard.AppServices.Contexts.Bulletins.Builders;
using BulletinBoard.AppServices.Contexts.Bulletins.Repositories;
using BulletinBoard.AppServices.Contexts.Categories.Services;
using BulletinBoard.AppServices.Contexts.Users.Services;
using BulletinBoard.AppServices.Exceptions;
using BulletinBoard.Contracts.Bulletins;
using Microsoft.Extensions.Logging;

namespace BulletinBoard.AppServices.Contexts.Bulletins.Services;

///<inheritdoc cref="IBulletinService"/>
public class BulletinService : IBulletinService
{
    private readonly IBulletinRepository _repository;
    private readonly IBulletinSpecificationBuilder _specificationBuilder;
    private readonly ICategoryService _categoryService;
    private readonly ILogger<BulletinService> _logger;
    private readonly IMapper _mapper;
    private readonly TimeProvider _timeProvider;
    private readonly IUserService _userService;

    /// <summary>
    /// Создаёт экземпляр <see cref="BulletinService"/>.
    /// </summary>
    /// <param name="repository">Репозиторий.</param>
    /// <param name="specificationBuilder">Билдер спецификаций.</param>
    /// <param name="categoryService">Сервис для работы с категориями.</param>
    /// <param name="logger">Логгер.</param>
    /// <param name="mapper">Маппер.</param>
    /// <param name="timeProvider">Провайдер времени.</param>
    /// <param name="userService">Сервис для работы с пользователями.</param>
    public BulletinService(IBulletinRepository repository, IBulletinSpecificationBuilder specificationBuilder,
        ICategoryService categoryService, ILogger<BulletinService> logger, IMapper mapper, TimeProvider timeProvider, IUserService userService)
    {
        _repository = repository;
        _specificationBuilder = specificationBuilder;
        _categoryService = categoryService;
        _logger = logger;
        _mapper = mapper;
        _timeProvider = timeProvider;
        _userService = userService;
    }

    /// <inheritdoc />
    public async Task<ICollection<BulletinDto>> SearchBulletinsAsync(SearchBulletinRequest request, CancellationToken cancellationToken)
    {
        using var _ = _logger.BeginScope("Поиск по запросу: {@Request}", request);
        var specification = _specificationBuilder.Build(request);
        _logger.LogInformation("Построена спецификация поиска объявлений");
        return await _repository.GetBySpecificationWithPaginationAsync(specification, request.Take, request.Skip, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ICollection<BulletinDto>> GetByCategoryAsync(int take, int? skip, Guid categoryId,
        CancellationToken cancellationToken)
    {
        using var _ = _logger.BeginScope("Поиск по категории: {id}", categoryId);
        var categories = await _categoryService.GetCategoryWithSubcategoriesAsync(categoryId, cancellationToken);
        var categoriesIds = categories.Select(c => c.Id).ToList();
        _logger.LogInformation("Получена категория с подкатегориями");
        var specification = _specificationBuilder.Build(categoriesIds);
        _logger.LogInformation("Построена спецификация поиска объявлений");
        var bulletins = await _repository.GetBySpecificationWithPaginationAsync(specification, take, skip,  cancellationToken);
        
        return bulletins;
    }

    /// <inheritdoc />
    public async Task<BulletinDto> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        _logger.BeginScope("Поиск по категории: {id}", id);
        return await _repository.GetByIdAsync(id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Guid> CreateAsync(Guid ownerId, CreateBulletinRequest request,
        CancellationToken cancellationToken)
    {
        _logger.BeginScope("Создание объявления по запросу: {@Request}, пользователем с id: {id}", request, ownerId);
        var bulletin = _mapper.Map<BulletinDto>(request);
        bulletin.OwnerId = ownerId;
        bulletin.Id = Guid.NewGuid();
        bulletin.CreatedAt = _timeProvider.GetUtcNow().UtcDateTime;
        
        return await _repository.CreateAsync(bulletin, cancellationToken);
    }

    /// <inheritdoc />
    public async Task UpdateAsync(Guid bulletinId, Guid userId, UpdateBulletinRequest request, CancellationToken cancellationToken)
    {
        _logger.BeginScope("Обновление объявления с id: {id}, по запросу: {@Request}", bulletinId, request);
        var bulletin = await _repository.GetByIdAsync(bulletinId, cancellationToken);
        if (bulletin.OwnerId != userId) throw new ForbiddenException();
        bulletin.Title = request.Title;
        bulletin.Description = request.Description;
        bulletin.CategoryId = request.CategoryId.Value;
        bulletin.Price = request.Price.Value;
        await _repository.UpdateAsync(bulletin, cancellationToken);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid bulletinId, Guid userId, CancellationToken cancellationToken)
    {
        var bulletin = await _repository.GetByIdAsync(bulletinId, cancellationToken);
        var user = await _userService.GetUserByIdAsync(userId, cancellationToken);
        if (!(bulletin.OwnerId == user.Id || user.Role == "Admin")) throw new ForbiddenException();
        _logger.BeginScope("Удаление объявления: {id}", bulletinId);
        await _repository.DeleteAsync(bulletinId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ICollection<BulletinDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        _logger.BeginScope("Получение всех объявлений");
        return await _repository.GetAllAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> IsUserBulletinsOwnerAsync(Guid bulletinId, Guid userId, CancellationToken cancellationToken)
    {
        var bulletin = await _repository.GetByIdAsync(bulletinId, cancellationToken);
        if (bulletin.OwnerId != userId) return false;
        return true;
    }
}