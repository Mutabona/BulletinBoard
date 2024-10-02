using System.Net;
using BulletinBoard.API.Controllers.Base;
using BulletinBoard.AppServices.Contexts.Users.Services;
using BulletinBoard.Contracts.Users;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoard.API.Controllers;

/// <summary>
/// Учетные записи.
/// </summary>
[ApiController]
[Route("[controller]")]
[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
public class AccountController(IUserService userService) : BaseController
{
    /// <summary>
    /// Регистрация пользователя.
    /// </summary>
    /// <param name="model">Модель регистрации пользователя.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Идентификатор нового пользователя.</returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserRequest model, CancellationToken cancellationToken)
    {
        var id = await userService.RegisterAsync(model, cancellationToken);

        if (id == Guid.Empty)
        {
            return BadRequest(new { message = "Username is already taken" });
        }
        
        return StatusCode((int)HttpStatusCode.Created, id.ToString());
    }

    /// <summary>
    /// Аутентификация пользователя
    /// </summary>
    /// <param name="model">Запрос на аутентификацию.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>JWT</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> LoginAsync([FromBody] LoginUserRequest model, CancellationToken cancellationToken)
    {
        var token = await userService.LoginAsync(model, cancellationToken);

        if (token == null)
        {
            return BadRequest(new { message = "Username or password is incorrect" });
        }
        
        return Ok(token);
    }
}