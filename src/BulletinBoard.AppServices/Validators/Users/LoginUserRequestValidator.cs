using BulletinBoard.AppServices.Contexts.Users.Services;
using BulletinBoard.Contracts.Users;
using FluentValidation;

namespace BulletinBoard.AppServices.Validators.Users;

/// <summary>
/// Валидатор запроса на вход пользователя.
/// </summary>
public class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
{
    private readonly IUserService _userService;

    /// <summary>
    /// Создаёт экземпляр <see cref="LoginUserRequestValidator"/>.
    /// </summary>
    /// <param name="userService">Сервис для работы с пользователями.</param>
    public LoginUserRequestValidator(IUserService userService)
    {
        _userService = userService;
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(50)
            .WithMessage("Некорректный пароль.");
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MinimumLength(2)
            .MaximumLength(50)
            .Must(UserExists)
            .WithMessage("Неверный логин.");
    }

    private bool UserExists(string login)
    {
        var user = _userService.GetUserByEmailAsync(login, CancellationToken.None).GetAwaiter().GetResult();
        
        if (user == null) return false;
        return true;
    }
}