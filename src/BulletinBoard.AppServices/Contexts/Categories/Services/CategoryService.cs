using System.Text.Json;
using BulletinBoard.AppServices.Contexts.Categories.Repositories;
using BulletinBoard.AppServices.Exceptions;
using BulletinBoard.Contracts.Categories;
using Microsoft.Extensions.Caching.Distributed;

namespace BulletinBoard.AppServices.Contexts.Categories.Services;

///<inheritdoc cref="ICategoryService"/>
public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;
    private readonly IDistributedCache _cache;
    
    private const string key = "all_categories";

    public CategoryService(ICategoryRepository repository, IDistributedCache cache)
    {
        _repository = repository;
        _cache = cache;
    }
    /// <inheritdoc />
    public async Task<Guid> CreateCategoryAsync(CreateCategoryRequest createCategoryRequest, CancellationToken cancellationToken)
    {
        await _cache.RemoveAsync(key, cancellationToken);
        return await _repository.AddCategoryAsync(createCategoryRequest, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<CategoryDto> GetCategoryByIdAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(categoryId, cancellationToken); 
    }

    /// <inheritdoc />
    public async Task DeleteCategoryAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        await _cache.RemoveAsync(key, cancellationToken);
        await _repository.DeleteAsync(categoryId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ICollection<CategoryDto>> GetCategoryWithSubcategoriesAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        var categories = await _repository.GetCategoryWithSubcategoriesAsync(categoryId, cancellationToken);
        return categories;
    }

    /// <inheritdoc />
    public async Task<ICollection<CategoryDto>> GetAllCategoriesAsync(CancellationToken cancellationToken)
    {
        ICollection<CategoryDto> categories;
        
        var serializedCategories = await _cache.GetStringAsync(key, cancellationToken);

        if (!string.IsNullOrEmpty(serializedCategories))
        {
            categories =  JsonSerializer.Deserialize<ICollection<CategoryDto>>(serializedCategories);
            return categories;
        }
        
        categories = await _repository.GetAllCategoriesAsync(cancellationToken);

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
        try
        {
            var category = await _repository.GetByIdAsync(categoryId, cancellationToken);
        }
        catch (EntityNotFoundException)
        {
            return false;
        }
        return true;
    }
}