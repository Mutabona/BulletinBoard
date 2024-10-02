﻿namespace BulletinBoard.Contracts.Users;

/// <summary>
/// Запрос на авторизацию пользователя.
/// </summary>
public class LoginUserRequest
{
    /// <summary>
    /// Логин.
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// Пароль.
    /// </summary>
    public string Password { get; set; }
}