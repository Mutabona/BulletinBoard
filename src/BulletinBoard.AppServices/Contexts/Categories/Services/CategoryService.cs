using BulletinBoard.AppServices.Contexts.Categories.Repositories;
using BulletinBoard.Contracts.Categories;

namespace BulletinBoard.AppServices.Contexts.Categories.Services;

///<inheritdoc cref="ICategoryService"/>
public class CategoryService(ICategoryRepository _repository) : ICategoryService
{
    /// <inheritdoc />
    public async Task<Guid> CreateCategoryAsync(CreateCategoryRequest createCategoryRequest, CancellationToken cancellationToken)
    {
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
        await _repository.DeleteAsync(categoryId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task UpdateCategoryAsync(CategoryDto category, CancellationToken cancellationToken)
    {
        await _repository.UpdateCategoryAsync(category, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ICollection<CategoryDto>> GetCategoryWithSubcategoriesAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        var categories = await _repository.GetCategoryWithSubcategoriesAsync(categoryId, cancellationToken);
        return categories;
    }
}