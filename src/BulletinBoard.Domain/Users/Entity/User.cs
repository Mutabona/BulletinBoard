using BulletinBoard.Domain.Base;
using BulletinBoard.Domain.Bulletins.Entity;

namespace BulletinBoard.Domain.Users.Entity;


/// <summary>
/// Сущность пользователя.
/// </summary>
public class User : BaseEntity
{
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
    /// Роль в системе.
    /// </summary>
    public string Role { get; set; }
    
    /// <summary>
    /// Объявления пользователя.
    /// </summary>
    public virtual ICollection<Bulletin> Bulletins { get; set; }
}