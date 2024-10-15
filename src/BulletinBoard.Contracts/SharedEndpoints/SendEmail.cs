namespace BulletinBoard.Contracts.Emails;

/// <summary>
/// Комманда отправки сообщения по почте.
/// </summary>
public class SendEmail
{
    /// <summary>
    /// Получаетль сообщения.
    /// </summary>
    public string Receiver { get; set; }
    
    /// <summary>
    /// Тема сообщения.
    /// </summary>
    public string Subject { get; set; }
    
    /// <summary>
    /// Сообщение.
    /// </summary>
    public string Text { get; set; }
}