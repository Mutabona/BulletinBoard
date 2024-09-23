namespace BulletinBoard.Contracts.Users;

/// <summary>
/// Пользователь.
/// </summary>
public class UserDto
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; set; }
   
    /// <summary>
    /// Дата создания пользователя.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Почта.
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// Номер телефона.
    /// </summary>
    public string Phone { get; set; }
    
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
    public string Lastname { get; set; }
    
    /// <summary>
    /// Роль в системе.
    /// </summary>
    public string Role { get; set; }
}