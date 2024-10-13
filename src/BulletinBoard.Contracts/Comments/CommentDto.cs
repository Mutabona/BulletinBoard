namespace BulletinBoard.Contracts.Comments;

/// <summary>
/// Модель комментария.
/// </summary>
public class CommentDto
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Дата создания сущности.
    /// </summary>
    public DateTime? CreatedAt { get; set; }
    
    /// <summary>
    /// Идентификатор автора комментария.
    /// </summary>
    public Guid AuthorId { get; set; }
    
    /// <summary>
    /// Имя автора.
    /// </summary>
    public string AuthorName { get; set; }
    
    /// <summary>
    /// Идентификатор объявления с комментарием.
    /// </summary>
    public Guid BulletinId { get; set; }
    
    /// <summary>
    /// Содержание комментария.
    /// </summary>
    public string Text { get; set; }
}