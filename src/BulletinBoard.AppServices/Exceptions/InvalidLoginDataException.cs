namespace BulletinBoard.AppServices.Exceptions;

/// <summary>
/// Ошибка "Неверное имя пользователя или пароль".
/// </summary>
public class InvalidLoginDataException : Exception
{
    public InvalidLoginDataException() : base("Неверное имя пользователя или пароль.")
    {
        
    }
}