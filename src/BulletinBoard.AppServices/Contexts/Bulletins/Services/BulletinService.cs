using BulletinBoard.AppServices.Contexts.Bulletins.Builders;
using BulletinBoard.AppServices.Contexts.Bulletins.Repositories;
using BulletinBoard.AppServices.Contexts.Categories.Services;
using BulletinBoard.Contracts.Bulletins;

namespace BulletinBoard.AppServices.Contexts.Bulletins.Services;

///<inheritdoc cref="IBulletinService"/>
public class BulletinService(IBulletinRepository _repository, IBulletinSpecificationBuilder _specificationBuilder, ICategoryService _categoryService) : IBulletinService
{
    /// <inheritdoc />
    public async Task<ICollection<BulletinDto>> SearchBulletinsAsync(SearchBulletinRequest request, CancellationToken cancellationToken)
    {
        var specification = _specificationBuilder.Build(request);
        return await _repository.GetBySpecificationWithPaginationAsync(specification, request.Take, request.Skip, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ICollection<BulletinDto>> GetByCategoryAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        var categories = await _categoryService.GetCategoryWithSubcategoriesAsync(categoryId, cancellationToken);
        var categoriesIds = categories.Select(c => c.Id).ToList();
        
        var specification = _specificationBuilder.Build(categoriesIds);
        var bulletins = await _repository.GetBySpecificationAsync(specification, cancellationToken);
        
        return bulletins;
    }

    /// <inheritdoc />
    public async Task<BulletinDto> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Guid> CreateAsync(CreateBulletinRequest request, CancellationToken cancellationToken)
    {
        return await _repository.CreateAsync(request, cancellationToken);
    }
}