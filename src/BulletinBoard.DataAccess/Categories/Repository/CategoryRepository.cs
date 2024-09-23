using AutoMapper;
using BulletinBoard.AppServices.Contexts.Categories.Repositories;
using BulletinBoard.Contracts.Categories;
using BulletinBoard.Domain.Categories.Entity;
using BulletinBoard.Infrastructure.Repository;

namespace BulletinBoard.DataAccess.Categories.Repository;

///<inheritdoc cref="ICategoryRepository"/>
public class CategoryRepository(IRepository<Category> _repository, IMapper _mapper) : ICategoryRepository
{
    ///<inheritdoc/>
    public async Task<Guid> AddCategoryAsync(CreateCategoryRequest category, CancellationToken cancellationToken)
    {
        var categoryEntity = _mapper.Map<Category>(category);
        return await _repository.AddAsync(categoryEntity, cancellationToken);
    }

    ///<inheritdoc/>
    public async Task<CategoryDto> GetByIdAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        return _mapper.Map<CategoryDto>(await _repository.GetByIdAsync(categoryId, cancellationToken));
    }

    ///<inheritdoc/>
    public async Task DeleteAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(categoryId, cancellationToken);
    }

    ///<inheritdoc/>
    public async Task UpdateCategoryAsync(CategoryDto category, CancellationToken cancellationToken)
    {
        await _repository.UpdateAsync(_mapper.Map<Category>(category), cancellationToken);
    }

    ///<inheritdoc/>
    public async Task<ICollection<CategoryDto>> GetCategoryWithSubcategoriesAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(categoryId, cancellationToken);
        
        var subcategories = new List<Category>();
        var subsubcategories = new List<Category>(category.Subcategories);
        var tempcategories = new List<Category>();
        
        subcategories.Add(category);

        while (subsubcategories.Count > 0)
        {
            foreach (var subsubcategory in subsubcategories)
            {
                foreach (var subsubsubcategory in subsubcategory.Subcategories)
                {
                    tempcategories.Add(subsubsubcategory);
                }
            }
            subcategories.AddRange(subsubcategories);
            subsubcategories.Clear();
            
            subsubcategories.AddRange(tempcategories);
            tempcategories.Clear();
        }
        
        return _mapper.Map<ICollection<CategoryDto>>(subcategories);
    }
}