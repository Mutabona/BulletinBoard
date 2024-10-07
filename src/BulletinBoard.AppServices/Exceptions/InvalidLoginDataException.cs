namespace BulletinBoard.AppServices.Exceptions;

/// <summary>
/// Ошибка неверных данных для аутентификации в системе.
/// </summary>
public class InvalidLoginDataException : HumanReadableException
{
    public InvalidLoginDataException(string humanReadableMessage) : base(humanReadableMessage)
    {
    }

    public InvalidLoginDataException(string message, string humanReadableMessage) : base(message, humanReadableMessage)
    {
    }
}