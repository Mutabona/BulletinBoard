using BulletinBoard.AppServices.Contexts.Bulletins.Services;
using BulletinBoard.AppServices.Contexts.Categories.Services;
using BulletinBoard.AppServices.Contexts.Users.Services;
using BulletinBoard.Contracts.Bulletins;
using FluentValidation;

namespace BulletinBoard.AppServices.Validators.Bulletins;

/// <summary>
/// Валидатор запроса на изменение категории.
/// </summary>
public class UpdateBulletinValidator : AbstractValidator<UpdateBulletinRequest>
{
    /// <summary>
    /// Создаёт экземпляр <see cref="UpdateBulletinValidator"/>.
    /// </summary>
    /// <param name="categoryService">Сервис для работы с категориями.</param>
    public UpdateBulletinValidator(ICategoryService categoryService)
    {
        RuleFor(x => x.Title)
            .MaximumLength(50)
            .MinimumLength(1)
            .NotEmpty();
        
        RuleFor(x => x.Description)
            .MaximumLength(1500);
        
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .Must(categoryId => categoryService.IsCategoryExistsAsync(categoryId.Value, CancellationToken.None).GetAwaiter().GetResult())
            .WithMessage("Неверная категория");
        
        RuleFor(x => x.Price)
            .NotNull()
            .GreaterThan(0)
            .WithMessage("Цена не может быть меньше или равна нулю.");
    }
}