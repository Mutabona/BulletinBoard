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
    private readonly IUserService _userService;
    
    /// <summary>
    /// Создаёт экземпляр <see cref="RegisterUserRequestValidator"/>.
    /// </summary>
    /// <param name="userService">Сервис для работы с пользователями.</param>
    public RegisterUserRequestValidator(IUserService userService)
    {
        _userService = userService;
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Пароль не может быть пустым.")
            .MinimumLength(2)
            .MaximumLength(50);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Логин не может быть пустым.")
            .EmailAddress()
            .WithMessage("Некорректный адрес.")
            .MinimumLength(2)
            .MaximumLength(50);

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Имя не может быть пустым.")
            .MinimumLength(1)
            .MaximumLength(50);
        
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