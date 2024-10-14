namespace BulletinBoard.AppServices.Exceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException() : base("Нет доступа") {}
}