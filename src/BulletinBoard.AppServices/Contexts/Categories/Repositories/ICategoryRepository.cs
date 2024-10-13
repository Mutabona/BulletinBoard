using BulletinBoard.Contracts.Categories;

namespace BulletinBoard.AppServices.Contexts.Categories.Repositories;

/// <summary>
/// Репозиторий для работы с категориями.
/// </summary>
public interface ICategoryRepository
{
    /// <summary>
    /// Добавление категории.
    /// </summary>
    /// <param name="category">Категория.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Идентификатор созданной категории.</returns>
    Task<Guid> AddCategoryAsync(CategoryDto category, CancellationToken cancellationToken);
    
    /// <summary>
    /// Получение категории по айди.
    /// </summary>
    /// <param name="categoryId">Айди.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Категория.</returns>
    Task<CategoryDto> GetByIdAsync(Guid categoryId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Удаляет категорию.
    /// </summary>
    /// <param name="categoryId">Идентификатор категории.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    Task DeleteAsync(Guid categoryId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Возвращает все вложенные категории по идентификатору.
    /// </summary>
    /// <param name="categoryId">Идентификатор корневой категории.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Коллекция категорий.</returns>
    Task<ICollection<CategoryDto>> GetCategoryWithSubcategoriesAsync(Guid categoryId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Возвращает все категории.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекция категорий.</returns>
    Task<ICollection<CategoryDto>> GetAllCategoriesAsync(CancellationToken cancellationToken);
}