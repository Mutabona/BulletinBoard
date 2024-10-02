namespace BulletinBoard.Contracts.Comments;

/// <summary>
/// Запрос на создание комментария.
/// </summary>
public class AddCommentRequest
{
    /// <summary>
    /// Содержание комментария.
    /// </summary>
    public string? Text { get; set; }
}