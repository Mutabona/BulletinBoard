namespace BulletinBoard.Contracts.Emails;

/// <summary>
/// Событие регистрации пользователя.
/// </summary>
public class UserRegistred
{
    /// <summary>
    /// Почта зарегистрированного пользователя.
    /// </summary>
    public string Email { get; set; }
}