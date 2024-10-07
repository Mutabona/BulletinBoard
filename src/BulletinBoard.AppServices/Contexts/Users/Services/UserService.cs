using BulletinBoard.AppServices.Contexts.Users.Repositories;
using BulletinBoard.AppServices.Exceptions;
using BulletinBoard.AppServices.Helpers;
using BulletinBoard.Contracts.Users;

namespace BulletinBoard.AppServices.Contexts.Users.Services;

///<inheritdoc cref="IUserService"/>
public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IJwtService _jwtService;

    public UserService(IUserRepository repository, IJwtService jwtService)
    {
        _repository = repository;
        _jwtService = jwtService;
    }
    
    ///<inheritdoc/>
    public async Task<ICollection<UserDto>> GetUsersAsync(CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync(cancellationToken);
    }

    ///<inheritdoc/>
    public async Task<UserDto> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(userId, cancellationToken);
    }

    ///<inheritdoc/>
    public async Task<UserDto> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _repository.GetByEmailAsync(email, cancellationToken);
    }

    ///<inheritdoc/>
    public async Task<Guid> RegisterAsync(RegisterUserRequest request, CancellationToken cancellationToken)
    {
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
        await _repository.DeleteAsync(userId, cancellationToken);
    }

    ///<inheritdoc/>
    public async Task<bool> IsUniqueEmailAsync(string email, CancellationToken cancellationToken)
    {
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