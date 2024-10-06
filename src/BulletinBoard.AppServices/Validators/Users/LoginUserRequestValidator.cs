using BulletinBoard.AppServices.Contexts.Users.Services;
using BulletinBoard.Contracts.Users;
using FluentValidation;

namespace BulletinBoard.AppServices.Validators.Users;

/// <summary>
/// Валидатор запроса на вход пользователя.
/// </summary>
public class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
{
    /// <summary>
    /// Создаёт экземпляр <see cref="LoginUserRequestValidator"/>.
    /// </summary>
    /// <param name="userService">Сервис для работы с пользователями.</param>
    public LoginUserRequestValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Пароль не может быть пустым.")
            .MinimumLength(6)
            .MaximumLength(50);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Логин не может быть пустым.")
            .EmailAddress()
            .WithMessage("Некорректный адрес.")
            .MinimumLength(2)
            .MaximumLength(50);
    }
}