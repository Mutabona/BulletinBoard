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
    private readonly ICategoryService _categoryService;
    
    /// <summary>
    /// Создаёт экземпляр <see cref="UpdateBulletinValidator"/>.
    /// </summary>
    /// <param name="categoryService">Сервис для работы с категориями.</param>
    public UpdateBulletinValidator(ICategoryService categoryService)
    {
        _categoryService = categoryService;

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Название не может быть пустым.")
            .MaximumLength(50)
            .MinimumLength(1);
            
            
        RuleFor(x => x.Description)
            .MaximumLength(1500)
            .WithMessage("Максимальная длинна описания 1500 символов.");
        
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Категория не может быть пустой.")
            .Must(CategoryIdIsValid)
            .WithMessage("Неверная категория.");
        
        RuleFor(x => x.Price)
            .NotNull()
            .WithMessage("Цена должна быть заполнена.")
            .GreaterThan(0)
            .WithMessage("Цена не может быть меньше или равна нулю.");
    }
    
    private bool CategoryIdIsValid(Guid? categoryId)
    {
        if (categoryId == null) return false;
        var answer = _categoryService.IsCategoryExistsAsync(categoryId.Value, CancellationToken.None).GetAwaiter()
            .GetResult();
        
        return answer;
    }
}