using BulletinBoard.Contracts.Users;

namespace BulletinBoard.AppServices.Contexts.Users.Services;

/// <summary>
/// Сервис для работы с пользователями.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Поучает всех пользователей.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекция пользователей.</returns>
    Task<ICollection<UserDto>> GetUsersAsync(CancellationToken cancellationToken);
    
    /// <summary>
    /// Получает пользователя по идентификатору.
    /// </summary>
    /// <param name="userId">Идентификатор.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Модель пользователя.</returns>
    Task<UserDto> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Регистрирует пользователя.
    /// </summary>
    /// <param name="request">Запрос на регистрацию.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Идентификатор зарегистрированного пользователя.</returns>
    Task<Guid> RegisterAsync(RegisterUserRequest request, CancellationToken cancellationToken);
    
    /// <summary>
    /// Запрос на авторизацию пользователя.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>JWT</returns>
    Task<string> LoginAsync(LoginUserRequest request, CancellationToken cancellationToken);
    
    /// <summary>
    /// Обновляет пользователя.
    /// </summary>
    /// <param name="user">Пользователь.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    Task UpdateUserAsync(UserDto user, CancellationToken cancellationToken);
    
    /// <summary>
    /// Удаляет пользователя по идентификатору.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Проверяет логин на уникальность.
    /// </summary>
    /// <param name="login">Логин.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>True, если логин уникальный, false, если нет</returns>
    Task<bool> IsUniqueLoginAsync(string login, CancellationToken cancellationToken);
}