using System.Data;
using System.Text.RegularExpressions;
using BulletinBoard.AppServices.Contexts.Users.Services;
using BulletinBoard.Contracts.Users;
using FluentValidation;

namespace BulletinBoard.AppServices.Validators.Users;

/// <summary>
/// Валидатор запроса на создание пользователя.
/// </summary>
public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    /// <summary>
    /// Создаёт экземпляр <see cref="RegisterUserRequestValidator"/>.
    /// </summary>
    /// <param name="userService">Сервис для работы с пользователями.</param>
    public RegisterUserRequestValidator(IUserService userService)
    {
        RuleFor(x => x.Password)
            .MinimumLength(2)
            .MaximumLength(50)
            .NotEmpty()
            .WithMessage("Пароль должен содержать от 2 до 50 символов");
        
        RuleFor(x => x.Email)
            .EmailAddress()
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50)
            .Must(email => userService.IsUniqueEmailAsync(email, CancellationToken.None).GetAwaiter().GetResult()).WithMessage("Некорректная почта.");
        
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(50)
            .WithMessage("Некорректное имя");
        
        RuleFor(x => x.Phone)
            .Must(ValidPhone)
            .WithMessage("Некорректный телефон");
    }

    private bool ValidPhone(string? phone)
    {
        if (phone is null) return true;
        
        if (Regex.IsMatch(phone, "[0-9]{11}")) return true;

        return false;
    }
}