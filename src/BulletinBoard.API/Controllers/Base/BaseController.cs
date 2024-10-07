using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoard.API.Controllers.Base;

/// <summary>
/// Базовый контроллер.
/// </summary>
public class BaseController : ControllerBase
{
    /// <summary>
    /// Получает идентификатор аутентифицированного пользователя.
    /// </summary>
    /// <returns>Идентификатор пользователя.</returns>
    protected Guid GetCurrentUserId()
    {
        var id = Guid.Parse(HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
        return id;
    }

    /// <summary>
    /// Получает роль авторизированного пользователя.
    /// </summary>
    /// <returns>Роль пользователя.</returns>
    protected string GetCurrentUserRole()
    {
        var role = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Role).Value;
        return role;
    }
}