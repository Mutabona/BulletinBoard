using BulletinBoard.AppServices.Contexts.Users.Repositories;
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
    public async Task<Guid> RegisterAsync(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        if (await IsUniqueLoginAsync(request.Login, cancellationToken))
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
        var user = await _repository.LoginAsync(request, cancellationToken);

        if (user != null)
        {
            return _jwtService.GetToken(request, user.Id, user.Role);
        }
        
        return null;
    }

    ///<inheritdoc/>
    public async Task UpdateUserAsync(UserDto user, CancellationToken cancellationToken)
    {
        await _repository.UpdateAsync(user, cancellationToken);
    }

    ///<inheritdoc/>
    public async Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(userId, cancellationToken);
    }

    ///<inheritdoc/>
    public async Task<bool> IsUniqueLoginAsync(string login, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByLoginAsync(login, cancellationToken);
        if (user == null) return true;
        return false;
    }
}