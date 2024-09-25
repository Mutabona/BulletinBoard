using System.Net;
using BulletinBoard.AppServices.Contexts.Categories.Services;
using BulletinBoard.Contracts.Categories;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoard.API.Controllers;

/// <summary>
/// Категории.
/// </summary>
[ApiController]
[Route("[controller]")]
[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
public class CategoryController(ICategoryService categoryService) : ControllerBase
{
    /// <summary>
    /// Создаёт категорию по модели запроса.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Идентификатор созданной категории.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> AddCategoryAsync(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
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
        var category = await categoryService.GetCategoryByIdAsync(categoryId, cancellationToken);
        
        if (category == null) return NotFound();
        
        return Ok(category);
    }

    /// <summary>
    /// Удаляет категори. по идентификатору.
    /// </summary>
    /// <param name="categoryId">Идентификатор.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    [HttpDelete("{categoryId}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteCategoryAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        await categoryService.DeleteCategoryAsync(categoryId, cancellationToken);

        return Ok();
    }

    /// <summary>
    /// Обновляет категорию.
    /// </summary>
    /// <param name="categoryDto">Модель категории.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    [HttpPut("{categoryId}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateCategoryAsync(CategoryDto categoryDto, CancellationToken cancellationToken)
    {
        await categoryService.UpdateCategoryAsync(categoryDto, cancellationToken);
        
        return Ok();
    }

    /// <summary>
    /// Возвращает дерево категорий по идентификатору корневой категории.
    /// </summary>
    /// <param name="categoryId">Идентификатор корневой категории.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекция моделей категорий.</returns>
    [HttpGet("{categoryId}/subcategories")]
    [ProducesResponseType(typeof(ICollection<CategoryDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetCategoryWithSubcategoriesAsync(Guid categoryId, CancellationToken cancellationToken)
    {
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