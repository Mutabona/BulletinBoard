using BulletinBoard.AppServices.Contexts.Categories.Services;
using BulletinBoard.Contracts.Bulletins;
using FluentValidation;

namespace BulletinBoard.AppServices.Validators.Bulletins;

/// <summary>
/// Валидатор запроса на создание объявления.
/// </summary>
public class CreateBulletinRequestValidator : AbstractValidator<CreateBulletinRequest>
{
    /// <summary>
    /// Создаёт экземпляр <see cref="CreateBulletinRequestValidator"/>
    /// </summary>
    /// <param name="categoryService">Сервис для работы с категориями.</param>
    public CreateBulletinRequestValidator(ICategoryService categoryService)
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(50)
            .WithMessage("Неподходящее название объявления");
        
        RuleFor(x => x.Description)
            .MaximumLength(1500)
            .WithMessage("Максимальная длинна описания 1500 символов.");
        
        RuleFor(x => x.Price)
            .NotNull().GreaterThan(0)
            .WithMessage("Цена не может быть меньше или равна нулю.");
        
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .Must(categoryId => categoryService.IsCategoryExistsAsync(categoryId.Value, CancellationToken.None).GetAwaiter().GetResult())
            .WithMessage("Неверная категория");
    }
}