using AutoMapper;
using AutoMapper.QueryableExtensions;
using BulletinBoard.AppServices.Contexts.Categories.Repositories;
using BulletinBoard.Contracts.Categories;
using BulletinBoard.Domain.Categories.Entity;
using BulletinBoard.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.DataAccess.Categories.Repository;

///<inheritdoc cref="ICategoryRepository"/>
public class CategoryRepository : ICategoryRepository
{
    private readonly IRepository<Category> _repository;
    private readonly IMapper _mapper;

    public CategoryRepository(IRepository<Category> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
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
        var category = await _repository.GetByIdAsync(categoryId, cancellationToken); //Получаем корневую категорию.
        
        var subcategories = new List<Category>(); //Категории, которые будем возвращать.
        var subsubcategories = new List<Category>(category.Subcategories); //Категории, которые сейчас будем обрабатывать.
        var tempcategories = new List<Category>(); //Категории, которые будем обрабатывать следующими.
        
        subcategories.Add(category);

        while (subsubcategories.Count > 0) //Выполнять, пока не останется не обработанных категорий.
        {
            foreach (var subsubcategory in subsubcategories) //Проходимся по категориям.
            {
                foreach (var subsubsubcategory in subsubcategory.Subcategories) //Проходимся по их дочерним категориям.
                {
                    tempcategories.Add(subsubsubcategory); //Добавляем каждую дочернюю категорию в список категорий, которые будут обработаны позже.
                }
            }
            subcategories.AddRange(subsubcategories); //Добавляем обработанные категории к тем, что будем возвращать.
            subsubcategories.Clear(); //Очищаем список обработанных категорий.
            
            subsubcategories.AddRange(tempcategories); //Перемещаем не обработанные категории в список для обработки
            tempcategories.Clear(); //Очищаем список для следующих категорий.
        }
        
        return _mapper.Map<ICollection<CategoryDto>>(subcategories);
    }

    ///<inheritdoc/>
    public async Task<ICollection<CategoryDto>> GetAllCategoriesAsync(CancellationToken cancellationToken)
    {
        return await _repository.GetAll().ProjectTo<CategoryDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
    }
}