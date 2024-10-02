﻿using System.Net;
using BulletinBoard.API.Controllers.Base;
using BulletinBoard.AppServices.Contexts.Users.Services;
using BulletinBoard.Contracts.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoard.API.Controllers;

/// <summary>
/// Пользователи.
/// </summary>
[ApiController]
[Route("[controller]")]
[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
public class UserController(IUserService userService) : BaseController
{
    /// <summary>
    /// Удаляет пользователя по идентификатору.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{userId}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> DeleteUser(Guid userId, CancellationToken cancellationToken)
    {
        await userService.DeleteUserAsync(userId, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Выполняет поиск пользователя по идентификатору.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Модель пользователя.</returns>
    [Authorize(Roles = "Admin")]
    [HttpGet("{userId}")]
    [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetUserById(Guid userId, CancellationToken cancellationToken)
    {
        var user = await userService.GetUserByIdAsync(userId, cancellationToken);
        
        if (user == null)
        {
            return NotFound();
        }
        
        return Ok(user);
    }

    /// <summary>
    /// Возвращает всех пользователей.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекция моделей пользователя.</returns>
    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    [ProducesResponseType(typeof(ICollection<UserDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        var users = await userService.GetUsersAsync(cancellationToken);
        return Ok(users);
    }
}