using BulletinBoard.AppServices.Contexts.Categories.Services;
using BulletinBoard.Contracts.Categories;
using FluentValidation;

namespace BulletinBoard.AppServices.Validators.Categories;

/// <summary>
/// Валидатор запроса на создание категории.
/// </summary>
public class CreateCategoryValidator : AbstractValidator<CreateCategoryRequest>
{
    private readonly ICategoryService _categoryService;
    
    /// <summary>
    /// Создаёт экземпляр <see cref="CreateCategoryValidator"/>.
    /// </summary>
    /// <param name="categoryService">Сервис для работы с категориями.</param>
    public CreateCategoryValidator(ICategoryService categoryService)
    {
        _categoryService = categoryService;

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50)
            .MinimumLength(2);
        
        RuleFor(x => x.ParentCategoryId)
            .Must((id) => ParentCategoryIdIsValid(id))
            .WithMessage("Неверная родительская категория.");
    }

    private bool ParentCategoryIdIsValid(Guid? parentCategoryId)
    {
        if (parentCategoryId == null) return true;
        
        return _categoryService.IsCategoryExistsAsync(parentCategoryId.Value, CancellationToken.None).GetAwaiter().GetResult();
    }
}