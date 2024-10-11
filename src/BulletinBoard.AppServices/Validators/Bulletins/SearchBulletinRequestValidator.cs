using BulletinBoard.AppServices.Contexts.Users.Services;
using BulletinBoard.AppServices.Exceptions;
using BulletinBoard.Contracts.Bulletins;
using FluentValidation;

namespace BulletinBoard.AppServices.Validators.Bulletins;

/// <summary>
/// Валидатор запроса на поиск объявлений.
/// </summary>
public class SearchBulletinRequestValidator : AbstractValidator<SearchBulletinRequest>
{
    private readonly IUserService _userService;

    public SearchBulletinRequestValidator(IUserService userService)
    {
        _userService = userService;

        RuleFor(x => x.Search)
            .MaximumLength(1500)
            .WithMessage("Превышена максимальная длинна текста поиска.");

        RuleFor(x => x.MaxPrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Максимальная цена не может быть меньше нуля.");
        
        RuleFor(x => x.MinPrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Минимальная цена не может быть меньше нуля.");

        RuleFor(x => x.Take)
            .GreaterThanOrEqualTo(0);
        
        RuleFor(x => x.Skip)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.UserId)
            .Must(UserExistsOrNull)
            .WithMessage("Такого пользователя не существует.");
    }

    private bool UserExistsOrNull(Guid? userId)
    {
        if (userId == null) return true;
        try
        {
            _userService.GetUserByIdAsync(userId.Value, CancellationToken.None).GetAwaiter().GetResult();
        }
        catch (EntityNotFoundException)
        {
            return false;
        }
        return true;
    }
}