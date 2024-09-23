using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BulletinBoard.Contracts.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BulletinBoard.AppServices.Contexts.Users.Services;

///<inheritdoc cref="IJwtService"/>
public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Создания экземпляра класса <see cref="JwtService"/>
    /// </summary>
    /// <param name="configuration">Конфигурация.</param>
    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    ///<inheritdoc/>
    public async Task<string> GetToken(LoginUserRequest userData, Guid id, string role)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userData.Login),
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Role, role),
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}