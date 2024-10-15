using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoFixture;
using BulletinBoard.AppServices.Services;
using BulletinBoard.Contracts.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Shouldly;

namespace BulletinBoard.Tests.AppServicesTests.Services;

public class JwtServiceTests
{
    private readonly Fixture _fixture;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<ILogger<JwtService>> _loggerMock;
    private readonly JwtService _jwtService;

    public JwtServiceTests()
    {
        _fixture = new Fixture();
        _configurationMock = new Mock<IConfiguration>();
        _loggerMock = new Mock<ILogger<JwtService>>();

        _jwtService = new JwtService(_configurationMock.Object, _loggerMock.Object);
    }

    [Fact]
    public void GetToken_ShouldReturnToken()
    {
        // Arrange
        var userData = _fixture.Create<LoginUserRequest>();
        var userId = _fixture.Create<Guid>();
        var role = "Admin";
        var key = "a_very_long_and_complex_key_12345678901234567890123456789012";

        _configurationMock.Setup(c => c["Jwt:Key"]).Returns(key);

        // Act
        var token = _jwtService.GetToken(userData, userId, role);

        // Assert
        token.ShouldNotBeNullOrEmpty();

        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };

        SecurityToken validatedToken;
        var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

        principal.Identity.Name.ShouldBe(userData.Email);
        principal.FindFirst(ClaimTypes.NameIdentifier).Value.ShouldBe(userId.ToString());
        principal.FindFirst(ClaimTypes.Role).Value.ShouldBe(role);
    }
}
