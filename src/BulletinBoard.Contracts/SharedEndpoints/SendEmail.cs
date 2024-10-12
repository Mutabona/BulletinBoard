namespace BulletinBoard.Contracts.Emails;

/// <summary>
/// Комманда отправки сообщения по почте.
/// </summary>
public class SendEmail
{
    /// <summary>
    /// Получаетль сообщения.
    /// </summary>
    public string receiver { get; set; }
    
    /// <summary>
    /// Тема сообщения.
    /// </summary>
    public string subject { get; set; }
    
    /// <summary>
    /// Сообщение.
    /// </summary>
    public string text { get; set; }
}