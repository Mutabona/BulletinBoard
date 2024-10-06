using BulletinBoard.Contracts.Comments;
using FluentValidation;

namespace BulletinBoard.AppServices.Validators.Comments;

/// <summary>
/// Валидатор запроса на добавление комментария.
/// </summary>
public class AddCommentValidator : AbstractValidator<AddCommentRequest>
{
    /// <summary>
    /// Создаёт экземпляр <see cref="AddCommentValidator"/>.
    /// </summary>
    public AddCommentValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty()
            .WithMessage("Комментарий не может быть пустым.")
            .MaximumLength(1500)
            .WithMessage("Максимальная длинна комментария 1500 символов.");
    }
}