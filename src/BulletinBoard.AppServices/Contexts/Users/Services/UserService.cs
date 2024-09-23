using BulletinBoard.AppServices.Contexts.Users.Repositories;
using BulletinBoard.AppServices.Helpers;
using BulletinBoard.Contracts.Users;

namespace BulletinBoard.AppServices.Contexts.Users.Services;

///<inheritdoc cref="IUserService"/>
public class UserService(IUserRepository repository, IJwtService jwtService) : IUserService
{
    ///<inheritdoc/>
    public async Task<ICollection<UserDto>> GetUsersAsync(CancellationToken cancellationToken)
    {
        return await repository.GetAllAsync(cancellationToken);
    }

    ///<inheritdoc/>
    public async Task<UserDto> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await repository.GetByIdAsync(userId, cancellationToken);
    }

    ///<inheritdoc/>
    public async Task<Guid> RegisterAsync(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        if (await IsUniqueLoginAsync(request.Login, cancellationToken))
        {
            request.Password = CryptoHelper.GetBase64Hash(request.Password);
            return await repository.AddAsync(request, cancellationToken);
        }

        return Guid.Empty;
    }

    ///<inheritdoc/>
    public async Task<string> LoginAsync(LoginUserRequest request, CancellationToken cancellationToken)
    {
        request.Password = CryptoHelper.GetBase64Hash(request.Password);
        var user = await repository.LoginAsync(request, cancellationToken);

        if (user != null)
        {
            return await jwtService.GetToken(request, user.Id, user.Role);
        }
        
        return null;
    }

    ///<inheritdoc/>
    public async Task UpdateUserAsync(UserDto user, CancellationToken cancellationToken)
    {
        await repository.UpdateAsync(user, cancellationToken);
    }

    ///<inheritdoc/>
    public async Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(userId, cancellationToken);
    }

    ///<inheritdoc/>
    public async Task<bool> IsUniqueLoginAsync(string login, CancellationToken cancellationToken)
    {
        var user = await repository.GetByLoginAsync(login, cancellationToken);
        if (user == null) return true;
        return false;
    }
}