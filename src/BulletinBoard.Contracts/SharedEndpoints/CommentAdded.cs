namespace BulletinBoard.Contracts.Emails;

/// <summary>
/// Событие добавления комментария.
/// </summary>
public class CommentAdded
{
    /// <summary>
    /// Идентификатор добавленного комментария.
    /// </summary>
    public Guid Id { get; set; }
}