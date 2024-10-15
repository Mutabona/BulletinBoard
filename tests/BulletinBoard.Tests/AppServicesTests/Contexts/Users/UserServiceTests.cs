using AutoFixture;
using AutoMapper;
using BulletinBoard.AppServices.Contexts.Users.Repositories;
using BulletinBoard.AppServices.Contexts.Users.Services;
using BulletinBoard.AppServices.Exceptions;
using BulletinBoard.AppServices.Services;
using BulletinBoard.ComponentRegistrar.MapProfiles;
using BulletinBoard.Contracts.Emails;
using BulletinBoard.Contracts.Users;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Time.Testing;
using Moq;
using Shouldly;

namespace BulletinBoard.Tests.AppServicesTests.Contexts.Users;

public class UserServiceTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IUserRepository> _repositoryMock;
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly Mock<ILogger<UserService>> _loggerMock;
    private readonly Mock<IPublishEndpoint> _publishEndpointMock;
    private readonly IMapper _mapper;
    private readonly UserService _userService;
    private readonly FakeTimeProvider _timeProvider;
    private readonly CancellationToken _token;

    public UserServiceTests()
    {
        var configurationProvider = new MapperConfiguration(delegate (IMapperConfigurationExpression configure)
        {
            configure.AddProfiles(new List<Profile>
            {
                new UserProfile(),
            });
        });
        
        _mapper = configurationProvider.CreateMapper();
        _fixture = new Fixture();
        _repositoryMock = new Mock<IUserRepository>();
        _jwtServiceMock = new Mock<IJwtService>();
        _loggerMock = new Mock<ILogger<UserService>>();
        _publishEndpointMock = new Mock<IPublishEndpoint>();
        _timeProvider = new FakeTimeProvider();
        _token = new CancellationToken();
        _userService = new UserService(_repositoryMock.Object, _jwtServiceMock.Object, _loggerMock.Object, _publishEndpointMock.Object, _mapper, _timeProvider);
    }

    [Fact]
    public async Task GetUsersAsync_Should_Return_All_Users()
    {
        // Arrange
        var users = _fixture.Create<ICollection<UserDto>>();
        _repositoryMock.Setup(x => x.GetAllAsync(_token)).ReturnsAsync(users);

        // Act
        var result = await _userService.GetUsersAsync(_token);

        // Assert
        result.ShouldBe(users);
    }

    [Fact]
    public async Task GetUserByIdAsync_Should_Return_User()
    {
        // Arrange
        var userId = _fixture.Create<Guid>();
        var user = _fixture.Create<UserDto>();
        _repositoryMock.Setup(x => x.GetByIdAsync(userId, _token)).ReturnsAsync(user);

        // Act
        var result = await _userService.GetUserByIdAsync(userId, _token);

        // Assert
        result.ShouldBe(user);
    }

    [Fact]
    public async Task GetUserByEmailAsync_Should_Return_User()
    {
        // Arrange
        var email = _fixture.Create<string>();
        var user = _fixture.Create<UserDto>();
        _repositoryMock.Setup(x => x.GetByEmailAsync(email, _token)).ReturnsAsync(user);

        // Act
        var result = await _userService.GetUserByEmailAsync(email, _token);

        // Assert
        result.ShouldBe(user);
    }

    [Fact]
    public async Task RegisterAsync_Should_Register_User()
    {
        // Arrange
        var registerUserRequest = _fixture.Create<RegisterUserRequest>();
        var utcNow = DateTime.UtcNow;

        _repositoryMock.Setup(x => x.AddAsync(It.IsAny<UserDto>(), _token)).ReturnsAsync(It.IsAny<Guid>());
        _timeProvider.SetUtcNow(utcNow);
        _repositoryMock.Setup(x => x.GetByEmailAsync(registerUserRequest.Email, _token)).ThrowsAsync(new EntityNotFoundException());

        // Act
        var result = await _userService.RegisterAsync(registerUserRequest, _token);

        // Assert
        result.ShouldBe(It.IsAny<Guid>());
        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<UserDto>(), _token), Times.Once);
        _publishEndpointMock.Verify(x => x.Publish(It.IsAny<UserRegistred>(), _token), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_Should_Throw_EmailAlreadyExistsException_If_Email_Is_Not_Unique()
    {
        // Arrange
        var registerUserRequest = _fixture.Create<RegisterUserRequest>();
        _repositoryMock.Setup(x => x.GetByEmailAsync(registerUserRequest.Email, _token)).ReturnsAsync(new UserDto());

        // Act & Assert
        await Should.ThrowAsync<EmailAlreadyExistsException>(() => _userService.RegisterAsync(registerUserRequest, _token));
    }

    [Fact]
    public async Task LoginAsync_Should_Return_Jwt_Token()
    {
        // Arrange
        var loginUserRequest = _fixture.Create<LoginUserRequest>();
        var user = _fixture.Create<UserDto>();
        var token = _fixture.Create<string>();

        _repositoryMock.Setup(x => x.LoginAsync(loginUserRequest, _token)).ReturnsAsync(user);
        _jwtServiceMock.Setup(x => x.GetToken(loginUserRequest, user.Id, user.Role)).Returns(token);

        // Act
        var result = await _userService.LoginAsync(loginUserRequest, _token);

        // Assert
        result.ShouldBe(token);
        _publishEndpointMock.Verify(x => x.Publish(It.IsAny<UserLoggedIn>(), _token), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_Should_Throw_InvalidLoginDataException_If_Login_Fails()
    {
        // Arrange
        var loginUserRequest = _fixture.Create<LoginUserRequest>();
        _repositoryMock.Setup(x => x.LoginAsync(loginUserRequest, _token)).ThrowsAsync(new EntityNotFoundException());

        // Act & Assert
        await Should.ThrowAsync<InvalidLoginDataException>(() => _userService.LoginAsync(loginUserRequest, _token));
    }

    [Fact]
    public async Task DeleteUserAsync_Should_Delete_User()
    {
        // Arrange
        var userId = _fixture.Create<Guid>();

        // Act
        await _userService.DeleteUserAsync(userId, _token);

        // Assert
        _repositoryMock.Verify(x => x.DeleteAsync(userId, _token), Times.Once);
    }

    [Fact]
    public async Task IsUniqueEmailAsync_Should_Return_True_If_Email_Is_Unique()
    {
        // Arrange
        var email = _fixture.Create<string>();
        _repositoryMock.Setup(x => x.GetByEmailAsync(email, _token)).ThrowsAsync(new EntityNotFoundException());

        // Act
        var result = await _userService.IsUniqueEmailAsync(email, _token);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public async Task IsUniqueEmailAsync_Should_Return_False_If_Email_Is_Not_Unique()
    {
        // Arrange
        var email = _fixture.Create<string>();
        _repositoryMock.Setup(x => x.GetByEmailAsync(email, _token)).ReturnsAsync(new UserDto());

        // Act
        var result = await _userService.IsUniqueEmailAsync(email, _token);

        // Assert
        result.ShouldBeFalse();
    }
}