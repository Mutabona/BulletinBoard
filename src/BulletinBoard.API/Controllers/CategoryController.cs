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
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddCategoryAsync(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var categoryId =  await _categoryService.CreateCategoryAsync(request, cancellationToken);

        return StatusCode((int)HttpStatusCode.Created, new { categoryId });
    }

    [HttpGet("{categoryId}")]
    public async Task<IActionResult> GetCategoryByIdAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        var category = await _categoryService.GetCategoryByIdAsync(categoryId, cancellationToken);
        
        return Ok(category);
    }

    [HttpDelete("{categoryId}")]
    public async Task<IActionResult> DeleteCategoryAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        await _categoryService.DeleteCategoryAsync(categoryId, cancellationToken);

        return Ok();
    }

    [HttpPut("{categoryId}")]
    public async Task<IActionResult> UpdateCategoryAsync(CategoryDto categoryDto, CancellationToken cancellationToken)
    {
        await _categoryService.UpdateCategoryAsync(categoryDto, cancellationToken);
        
        return Ok();
    }

    [HttpGet("{categoryId}/subcategories")]
    public async Task<IActionResult> GetCategoryWithSubcategoriesAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        var categories = await _categoryService.GetCategoryWithSubcategoriesAsync(categoryId, cancellationToken);
        
        return Ok(categories);
    }
}