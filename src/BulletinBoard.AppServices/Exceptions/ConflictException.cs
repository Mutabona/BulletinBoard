namespace BulletinBoard.AppServices.Exceptions;

public class ConflictException : Exception
{
    public ConflictException() : base("Conflict") { }
}