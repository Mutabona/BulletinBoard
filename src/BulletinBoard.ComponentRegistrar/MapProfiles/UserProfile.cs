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
        CreateMap<RegisterUserRequest, UserDto>(MemberList.None)
            .ForMember(s => s.Id, map => map.MapFrom(src => Guid.NewGuid()))
            .ForMember(s => s.CreatedAt, map => map.MapFrom(src => DateTime.UtcNow));
        
        CreateMap<User, UserDto>(MemberList.None).ReverseMap();
    }
}