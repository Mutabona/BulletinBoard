namespace BulletinBoard.AppServices.Exceptions;

public class EmailAlreadyExistsException : HumanReadableException
{
    public EmailAlreadyExistsException(string humanReadableMessage) : base(humanReadableMessage)
    {
    }

    public EmailAlreadyExistsException(string message, string humanReadableMessage) : base(message, humanReadableMessage)
    {
    }
}