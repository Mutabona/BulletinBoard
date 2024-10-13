using System.Net;
using BulletinBoard.API.Controllers.Base;
using BulletinBoard.AppServices.Contexts.Categories.Services;
using BulletinBoard.AppServices.Exceptions;
using BulletinBoard.Contracts.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoard.API.Controllers;

/// <summary>
/// Категории.
/// </summary>
[ApiController]
[Route("[controller]")]
[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
public class CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger) : BaseController
{
    /// <summary>
    /// Создаёт категорию по модели запроса.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Идентификатор созданной категории.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> AddCategoryAsync(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Добавление категории: {@Request}", request);
        var categoryId =  await categoryService.CreateCategoryAsync(request, cancellationToken);

        return StatusCode((int)HttpStatusCode.Created,  categoryId.ToString() );
    }

    /// <summary>
    /// Выполняет поиск категории по идентификатору.
    /// </summary>
    /// <param name="categoryId">Идентификатор категории.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Модель категории.</returns>
    [HttpGet("{categoryId}")]
    [ProducesResponseType(typeof(CategoryDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetCategoryByIdAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Поиск категории: {id}", categoryId);
        var category = await categoryService.GetCategoryByIdAsync(categoryId, cancellationToken);
        
        return Ok(category);
    }

    /// <summary>
    /// Удаляет категорию по идентификатору.
    /// </summary>
    /// <param name="categoryId">Идентификатор.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    [HttpDelete("{categoryId}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> DeleteCategoryAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Удаление категории: {id}", categoryId);
        await categoryService.DeleteCategoryAsync(categoryId, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Возвращает дерево категорий по идентификатору корневой категории.
    /// </summary>
    /// <param name="categoryId">Идентификатор корневой категории.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекция моделей категорий.</returns>
    [HttpGet("{categoryId}/subcategories")]
    [ProducesResponseType(typeof(ICollection<CategoryDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetCategoryWithSubcategoriesAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        await categoryService.GetCategoryWithSubcategoriesAsync(categoryId, cancellationToken);

        logger.LogInformation("Получение категории с подкатегориями: {id}", categoryId);
        var categories = await categoryService.GetCategoryWithSubcategoriesAsync(categoryId, cancellationToken);
        
        return Ok(categories);
    }

    /// <summary>
    /// Возвращает все категории.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекция категорий.</returns>
    [HttpGet("all")]
    [ProducesResponseType(typeof(ICollection<CategoryDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllCategoriesAsync(CancellationToken cancellationToken)
    {
        var categories = await categoryService.GetAllCategoriesAsync(cancellationToken);
        
        return Ok(categories);
    }
}