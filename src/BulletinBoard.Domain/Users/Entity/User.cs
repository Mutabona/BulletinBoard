using BulletinBoard.Domain.Base;
using BulletinBoard.Domain.Bulletins.Entity;
using BulletinBoard.Domain.Comments.Entity;

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
    /// Почта.
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// Номер телефона.
    /// </summary>
    public string? Phone { get; set; }
    
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
    
    /// <summary>
    /// Комментарии пользователя.
    /// </summary>
    public virtual ICollection<Comment> Comments { get; set; }
}