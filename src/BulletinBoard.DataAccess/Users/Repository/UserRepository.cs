using AutoMapper;
using AutoMapper.QueryableExtensions;
using BulletinBoard.AppServices.Contexts.Users.Repositories;
using BulletinBoard.Contracts.Users;
using BulletinBoard.Domain.Users.Entity;
using BulletinBoard.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.DataAccess.Users.Repository;

///<inheritdoc cref="IUserRepository"/>
public class UserRepository(IRepository<User> repository, IMapper mapper) : IUserRepository
{
    ///<inheritdoc/>
    public async Task<Guid> AddAsync(RegisterUserRequest user, CancellationToken cancellationToken)
    {
        var userEntity = mapper.Map<User>(user);
        return await repository.AddAsync(userEntity, cancellationToken);
    }

    public async Task<UserDto> LoginAsync(LoginUserRequest request, CancellationToken cancellationToken)
    { 
        return await repository.GetAll().Where(s => s.Login == request.Login && s.Password == request.Password)
            .ProjectTo<UserDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<UserDto> GetByLoginAsync(string login, CancellationToken cancellationToken)
    {
        return await repository.GetAll().Where(u => u.Login == login).ProjectTo<UserDto>(mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);
    }

    ///<inheritdoc/>
    public async Task<UserDto> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var userEntity = mapper.Map<UserDto>(await repository.GetByIdAsync(userId, cancellationToken));
        return userEntity;
    }

    ///<inheritdoc/>
    public async Task<ICollection<UserDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await repository.GetAll().ProjectTo<UserDto>(mapper.ConfigurationProvider).ToListAsync(cancellationToken);
    }

    ///<inheritdoc/>
    public async Task UpdateAsync(UserDto user, CancellationToken cancellationToken)
    {
        var userEntity = mapper.Map<User>(user);
        await repository.UpdateAsync(userEntity, cancellationToken); 
    }

    ///<inheritdoc/>
    public async Task DeleteAsync(Guid userId, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(userId, cancellationToken);
    }
}