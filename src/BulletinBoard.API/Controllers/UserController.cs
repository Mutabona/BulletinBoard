using System.Net;
using BulletinBoard.AppServices.Contexts.Users.Services;
using BulletinBoard.Contracts.Users;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoard.API.Controllers;

/// <summary>
/// Пользователи.
/// </summary>
[ApiController]
[Route("[controller]")]
[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(Guid userId, CancellationToken cancellationToken)
    {
        await _userService.DeleteUserAsync(userId, cancellationToken);
        return Ok();
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByIdAsync(userId, cancellationToken);
        
        if (user == null)
        {
            return NotFound();
        }
        
        return Ok(user);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        var users = await _userService.GetUsersAsync(cancellationToken);
        return Ok(users);
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUser(UserDto user, CancellationToken cancellationToken)
    {
        await _userService.UpdateUserAsync(user, cancellationToken);

        return Ok();
    }
}