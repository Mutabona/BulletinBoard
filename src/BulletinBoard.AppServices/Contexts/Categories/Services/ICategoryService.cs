using BulletinBoard.Contracts.Categories;

namespace BulletinBoard.AppServices.Contexts.Categories.Services;

/// <summary>
/// Сервис для работы с категориями.
/// </summary>
public interface ICategoryService
{
    /// <summary>
    /// Создаёт новую категорию по запросу.
    /// </summary>
    /// <param name="createCategoryRequest">Запрос на создание категории.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Идентификатор созданной категории.</returns>
    Task<Guid> CreateCategoryAsync(CreateCategoryRequest createCategoryRequest, CancellationToken cancellationToken);
    
    /// <summary>
    /// Возвращает категорию по идентификатору.
    /// </summary>
    /// <param name="categoryId">Идентификатор.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Модель категории.</returns>
    Task<CategoryDto> GetCategoryByIdAsync(Guid categoryId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Удаляет категорию по идентификатору.
    /// </summary>
    /// <param name="categoryId">Идентификатор.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    Task DeleteCategoryAsync(Guid categoryId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Обновляет категорию.
    /// </summary>
    /// <param name="category">Категория.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    Task UpdateCategoryAsync(CategoryDto category, CancellationToken cancellationToken);
    
    /// <summary>
    /// Получает дочерние категории.
    /// </summary>
    /// <param name="categoryId">Идентификатор категории.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекия категорий.</returns>
    Task<ICollection<CategoryDto>> GetCategoryWithSubcategoriesAsync(Guid categoryId, CancellationToken cancellationToken);
}