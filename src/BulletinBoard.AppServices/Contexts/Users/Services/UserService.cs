using BulletinBoard.AppServices.Contexts.Users.Repositories;
using BulletinBoard.AppServices.Exceptions;
using BulletinBoard.AppServices.Helpers;
using BulletinBoard.Contracts.Users;
using Microsoft.Extensions.Logging;

namespace BulletinBoard.AppServices.Contexts.Users.Services;

///<inheritdoc cref="IUserService"/>
public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IJwtService _jwtService;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository repository, IJwtService jwtService, ILogger<UserService> logger)
    {
        _repository = repository;
        _jwtService = jwtService;
        _logger = logger;
    }
    
    ///<inheritdoc/>
    public async Task<ICollection<UserDto>> GetUsersAsync(CancellationToken cancellationToken)
    {
        _logger.BeginScope("Получение всех пользователей");
        return await _repository.GetAllAsync(cancellationToken);
    }

    ///<inheritdoc/>
    public async Task<UserDto> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        _logger.BeginScope("Поиск пользователя: {id}", userId);
        return await _repository.GetByIdAsync(userId, cancellationToken);
    }

    ///<inheritdoc/>
    public async Task<UserDto> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        _logger.BeginScope("Поиск пользователя по почте: {email}", email);
        return await _repository.GetByEmailAsync(email, cancellationToken);
    }

    ///<inheritdoc/>
    public async Task<Guid> RegisterAsync(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        _logger.BeginScope("Запрос на регистрацию: {@Request}", request);
        if (await IsUniqueEmailAsync(request.Email, cancellationToken))
        {
            request.Password = CryptoHelper.GetBase64Hash(request.Password);
            return await _repository.AddAsync(request, cancellationToken);
        }

        return Guid.Empty;
    }

    ///<inheritdoc/>
    public async Task<string> LoginAsync(LoginUserRequest request, CancellationToken cancellationToken)
    {
        _logger.BeginScope("Запрос на вход: {@Request}", request);
        request.Password = CryptoHelper.GetBase64Hash(request.Password);
        UserDto user;
        try
        {
            user = await _repository.LoginAsync(request, cancellationToken);
        }
        catch (EntityNotFoundException)
        {
            throw new InvalidLoginDataException("Неверное имя пользователя или пароль.");
        }
        
        return _jwtService.GetToken(request, user.Id, user.Role);
    }

    ///<inheritdoc/>
    public async Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        _logger.BeginScope("Удаление пользователя: {id}", userId);
        await _repository.DeleteAsync(userId, cancellationToken);
    }

    ///<inheritdoc/>
    public async Task<bool> IsUniqueEmailAsync(string email, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Проверка на уникальность почты: {email}", email);
        try
        {
            var user = await _repository.GetByEmailAsync(email, cancellationToken);
        }
        catch (EntityNotFoundException)
        {
            return true;
        }
        return false;
    }
}