namespace BulletinBoard.Contracts.Users;

/// <summary>
/// Запрос на регистрацию пользователя.
/// </summary>
public class RegisterUserRequest
{
    /// <summary>
    /// Почта.
    /// </summary>
    public string? Email { get; set; }
    
    /// <summary>
    /// Номер телефона.
    /// </summary>
    public string? Phone { get; set; }
    
    /// <summary>
    /// Логин.
    /// </summary>
    public string Login { get; set; }
    
    /// <summary>
    /// Пароль.
    /// </summary>
    public string Password { get; set; }
    
    /// <summary>
    /// Имя.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Фамилия.
    /// </summary>
    public string Surname { get; set; }
    
    /// <summary>
    /// Отчество. 
    /// </summary>
    public string? Lastname { get; set; }
}