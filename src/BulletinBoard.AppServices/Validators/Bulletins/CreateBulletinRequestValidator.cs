using BulletinBoard.AppServices.Contexts.Categories.Services;
using BulletinBoard.Contracts.Bulletins;
using FluentValidation;

namespace BulletinBoard.AppServices.Validators.Bulletins;

/// <summary>
/// Валидатор запроса на создание объявления.
/// </summary>
public class CreateBulletinRequestValidator : AbstractValidator<CreateBulletinRequest>
{
    private readonly ICategoryService _categoryService;
    
    /// <summary>
    /// Создаёт экземпляр <see cref="CreateBulletinRequestValidator"/>
    /// </summary>
    /// <param name="categoryService">Сервис для работы с категориями.</param>
    public CreateBulletinRequestValidator(ICategoryService categoryService)
    {
        _categoryService = categoryService;
        
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Название не может быть пустым.")
            .MinimumLength(1)
            .MaximumLength(50)
            .WithMessage("Неподходящее название объявления.");
        
        RuleFor(x => x.Description)
            .MaximumLength(1500)
            .WithMessage("Максимальная длинна описания 1500 символов.");
        
        RuleFor(x => x.Price)
            .NotNull()
            .WithMessage("Цена должна быть заполнена.")
            .GreaterThan(0)
            .WithMessage("Цена не может быть меньше или равна нулю.");
        
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Категория не может быть пустой.")
            .Must(CategoryIdIsValid)
            .WithMessage("Неверная категория.");
    }

    private bool CategoryIdIsValid(Guid? categoryId)
    {
        if (categoryId == null) return false;
        return _categoryService.IsCategoryExistsAsync(categoryId.Value, CancellationToken.None).GetAwaiter()
            .GetResult();
    }
}