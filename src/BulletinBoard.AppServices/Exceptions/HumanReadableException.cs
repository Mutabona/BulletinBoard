namespace BulletinBoard.AppServices.Exceptions;

/// <summary>
/// Базовый класс человеко-читаемых ошибок.
/// </summary>
public abstract class HumanReadableException : Exception
{
    /// <summary>
    /// Человеко-читаемое сообщение.
    /// </summary>
    public string HumanReadableMessage { get; }

    public HumanReadableException(string humanReadableMessage)
    {
        HumanReadableMessage = humanReadableMessage;
    }

    public HumanReadableException(string message, string humanReadableMessage) : base(message)
    {
        HumanReadableMessage = humanReadableMessage;
    }
}