namespace BulletinBoard.Contracts.Emails;

/// <summary>
/// Событие аутентификации пользователя.
/// </summary>
public class UserLoggedIn
{
    /// <summary>
    /// Почта по которой вошли в систему.
    /// </summary>
    public string Email { get; set; }
}