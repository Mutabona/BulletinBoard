using AutoMapper;
using BulletinBoard.Contracts.Users;
using BulletinBoard.Domain.Users.Entity;

namespace BulletinBoard.ComponentRegistrar.MapProfiles;

/// <summary>
/// Профиль пользователя.
/// </summary>
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterUserRequest, UserDto>(MemberList.None);
        
        CreateMap<User, UserDto>(MemberList.None).ReverseMap();
    }
}