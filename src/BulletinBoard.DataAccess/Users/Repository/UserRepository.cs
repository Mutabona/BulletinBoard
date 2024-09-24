using AutoMapper;
using AutoMapper.QueryableExtensions;
using BulletinBoard.AppServices.Contexts.Users.Repositories;
using BulletinBoard.Contracts.Users;
using BulletinBoard.Domain.Users.Entity;
using BulletinBoard.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.DataAccess.Users.Repository;

///<inheritdoc cref="IUserRepository"/>
public class UserRepository : IUserRepository
{
    private readonly IRepository<User> _repository;
    private readonly IMapper _mapper;

    public UserRepository(IRepository<User> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    ///<inheritdoc/>
    public async Task<Guid> AddAsync(RegisterUserRequest user, CancellationToken cancellationToken)
    {
        var userEntity = _mapper.Map<User>(user);
        userEntity.Role = "User";
        return await _repository.AddAsync(userEntity, cancellationToken);
    }

    public async Task<UserDto> LoginAsync(LoginUserRequest request, CancellationToken cancellationToken)
    { 
        return await _repository.GetAll().Where(s => s.Login == request.Login && s.Password == request.Password)
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<UserDto> GetByLoginAsync(string login, CancellationToken cancellationToken)
    {
        return await _repository.GetAll().Where(u => u.Login == login).ProjectTo<UserDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);
    }

    ///<inheritdoc/>
    public async Task<UserDto> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var userEntity = _mapper.Map<UserDto>(await _repository.GetByIdAsync(userId, cancellationToken));
        return userEntity;
    }

    ///<inheritdoc/>
    public async Task<ICollection<UserDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _repository.GetAll().ProjectTo<UserDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
    }

    ///<inheritdoc/>
    public async Task UpdateAsync(UserDto user, CancellationToken cancellationToken)
    {
        var userEntity = _mapper.Map<User>(user);
        await _repository.UpdateAsync(userEntity, cancellationToken); 
    }

    ///<inheritdoc/>
    public async Task DeleteAsync(Guid userId, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(userId, cancellationToken);
    }
}