using System.Net;
using BulletinBoard.AppServices.Contexts.Users.Services;
using BulletinBoard.Contracts.Users;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoard.API.Controllers;

[ApiController]
[Route("[controller]")]
[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
public class AccountsController : ControllerBase
{
    private readonly IUserService _userService;

    public AccountsController(IUserService userService)
    {
        _userService = userService;
    }
    
    /// <summary>
    /// Регистрация пользователя.
    /// </summary>
    /// <param name="model">Модель регистрации пользователя.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Идентификатор нового пользователя.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserRequest model, CancellationToken cancellationToken)
    {
        var id = await _userService.RegisterAsync(model, cancellationToken);

        if (id == Guid.Empty)
        {
            return BadRequest(new { message = "Username is already taken" });
        }
        
        return Ok(id);
    }

    /// <summary>
    /// Аутентификация пользователя
    /// </summary>
    /// <param name="model">Запрос на аутентификацию.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>JWT</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> LoginAsync([FromBody] LoginUserRequest model, CancellationToken cancellationToken)
    {
        var token = await _userService.LoginAsync(model, cancellationToken);

        if (token == null)
        {
            return BadRequest(new { message = "Username or password is incorrect" });
        }
        
        return Ok(token);
    }
}