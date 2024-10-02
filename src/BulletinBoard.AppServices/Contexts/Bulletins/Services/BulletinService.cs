using BulletinBoard.AppServices.Contexts.Bulletins.Builders;
using BulletinBoard.AppServices.Contexts.Bulletins.Repositories;
using BulletinBoard.AppServices.Contexts.Categories.Services;
using BulletinBoard.Contracts.Bulletins;

namespace BulletinBoard.AppServices.Contexts.Bulletins.Services;

///<inheritdoc cref="IBulletinService"/>
public class BulletinService : IBulletinService
{
    private readonly IBulletinRepository _repository;
    private readonly IBulletinSpecificationBuilder _specificationBuilder;
    private readonly ICategoryService _categoryService;

    public BulletinService(IBulletinRepository repository, IBulletinSpecificationBuilder specificationBuilder,
        ICategoryService categoryService)
    {
        _repository = repository;
        _specificationBuilder = specificationBuilder;
        _categoryService = categoryService;
    }

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
    public async Task<Guid> CreateAsync(Guid ownerId, CreateBulletinRequest request,
        CancellationToken cancellationToken)
    {
        return await _repository.CreateAsync(ownerId, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task UpdateAsync(Guid bulletinId, UpdateBulletinRequest request, CancellationToken cancellationToken)
    {
        await _repository.UpdateAsync(bulletinId, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ICollection<BulletinDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync(cancellationToken);
    }
}