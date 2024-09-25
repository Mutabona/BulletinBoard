using BulletinBoard.Contracts.Users;

namespace BulletinBoard.AppServices.Contexts.Users.Services;

/// <summary>
/// Сервис для работы с jwt токенами.
/// </summary>
public interface IJwtService
{
    /// <summary>
    /// Создание jwt токена.
    /// </summary>
    /// <param name="userData">Запрос на создание <see cref="LoginRequest"/>.</param>
    /// <param name="id">Идентификатор польззователя.</param>
    /// <param name="role">Роль пользоваетеля.</param>
    /// <returns>Токен в виде строки.</returns>
    string GetToken(LoginUserRequest userData, Guid id, string role);
}