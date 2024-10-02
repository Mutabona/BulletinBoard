namespace BulletinBoard.Contracts.Comments;

/// <summary>
/// Модель комментария.
/// </summary>
public class CommentDto
{
    /// <summary>
    /// Идентификатор автора комментария.
    /// </summary>
    public Guid? AuthorId { get; private set; }
    
    /// <summary>
    /// Идентификатор объявления с комментарием.
    /// </summary>
    public Guid? BulletinId { get; private set; }
    
    /// <summary>
    /// Содержание комментария.
    /// </summary>
    public string Text { get; private set; }
}