namespace BulletinBoard.AppServices.Exceptions;

/// <summary>
/// Ошибка "Эта почта уже зарегистрирована".
/// </summary>
public class EmailAlreadyExistsException : Exception
{
    public EmailAlreadyExistsException() : base("Эта почта уже зарегистрирована.")
    {
        
    }
}