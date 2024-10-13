using System.Text.Json;
using BulletinBoard.AppServices.Contexts.Categories.Repositories;
using BulletinBoard.AppServices.Exceptions;
using BulletinBoard.Contracts.Categories;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace BulletinBoard.AppServices.Contexts.Categories.Services;

///<inheritdoc cref="ICategoryService"/>
public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;
    private readonly IDistributedCache _cache;
    private readonly ILogger<CategoryService> _logger;
    
    private const string key = "all_categories";

    /// <summary>
    /// Создаёт экземпляр <see cref="CategoryService"/>.
    /// </summary>
    /// <param name="repository">Репозиторий.</param>
    /// <param name="cache">Кэш.</param>
    /// <param name="logger">Логгер.</param>
    public CategoryService(ICategoryRepository repository, IDistributedCache cache, ILogger<CategoryService> logger)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
    }
    
    /// <inheritdoc />
    public async Task<Guid> CreateCategoryAsync(CreateCategoryRequest createCategoryRequest, CancellationToken cancellationToken)
    {
        _logger.BeginScope("Добавление категории: {@Request}", createCategoryRequest);
        var categoryId = await _repository.AddCategoryAsync(createCategoryRequest, cancellationToken);
        _logger.LogInformation("Очистка кеша по ключу: {key}", key);
        await _cache.RemoveAsync(key, cancellationToken);
        return categoryId;
    }

    /// <inheritdoc />
    public async Task<CategoryDto> GetCategoryByIdAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        _logger.BeginScope("Поиск категории: {id}", categoryId);
        return await _repository.GetByIdAsync(categoryId, cancellationToken); 
    }

    /// <inheritdoc />
    public async Task DeleteCategoryAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        _logger.BeginScope("Удаление категории: {id}", categoryId);
        _logger.LogInformation("Очистка кеша по ключу: {key}", key);
        await _cache.RemoveAsync(key, cancellationToken);
        await _repository.DeleteAsync(categoryId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ICollection<CategoryDto>> GetCategoryWithSubcategoriesAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        _logger.BeginScope("Получение категории с подкатегориями: {id}", categoryId);
        var categories = await _repository.GetCategoryWithSubcategoriesAsync(categoryId, cancellationToken);
        return categories;
    }

    /// <inheritdoc />
    public async Task<ICollection<CategoryDto>> GetAllCategoriesAsync(CancellationToken cancellationToken)
    {
        _logger.BeginScope("Получение всех категорий");
        ICollection<CategoryDto> categories;
        
        _logger.LogInformation("Проверка наличия категорий в кеше");
        var serializedCategories = await _cache.GetStringAsync(key, cancellationToken);

        if (!string.IsNullOrEmpty(serializedCategories))
        {
            _logger.LogInformation("Получение категорий из кеша");
            categories =  JsonSerializer.Deserialize<ICollection<CategoryDto>>(serializedCategories);
            return categories;
        }
        _logger.LogInformation("В кеше нет категорий");
        _logger.LogInformation("Получение категорий из репозитория");
        categories = await _repository.GetAllCategoriesAsync(cancellationToken);

        _logger.LogInformation("Сохранение категорий в кеше");
        serializedCategories = JsonSerializer.Serialize(categories);
        await _cache.SetStringAsync(key, serializedCategories, 
            new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromHours(1)
            }, cancellationToken);
        
        return categories;
    }

    /// <inheritdoc />
    public async Task<bool> IsCategoryExistsAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Проверка существования категории: {id}", categoryId);
        try
        {
            var category = await _repository.GetByIdAsync(categoryId, cancellationToken);
        }
        catch (EntityNotFoundException)
        {
            _logger.LogInformation("Категория не существует");
            return false;
        }
        _logger.LogInformation("Категория существует");
        return true;
    }
}