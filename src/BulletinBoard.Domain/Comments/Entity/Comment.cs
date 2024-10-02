using BulletinBoard.Domain.Base;
using BulletinBoard.Domain.Bulletins.Entity;
using BulletinBoard.Domain.Users.Entity;

namespace BulletinBoard.Domain.Comments.Entity;

/// <summary>
/// Сущность комментария.
/// </summary>
public class Comment : BaseEntity
{
    /// <summary>
    /// Идентификатор автора комментария.
    /// </summary>
    public Guid AuthorId { get; set; }
    
    /// <summary>
    /// Автор комментария.
    /// </summary>
    public virtual User Author { get; set; }
    
    /// <summary>
    /// Идентификатор объявления с комментарием.
    /// </summary>
    public Guid BulletinId { get; set; }
    
    /// <summary>
    /// Объявление с комментарием.
    /// </summary>
    public virtual Bulletin Bulletin { get; set; }
    
    /// <summary>
    /// Содержание комментария.
    /// </summary>
    public string Text { get; set; }
}