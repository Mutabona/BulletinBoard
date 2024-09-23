using AutoMapper;
using BulletinBoard.Contracts.Users;
using BulletinBoard.Domain.Users.Entity;

namespace BulletinBoard.ComponentRegistrar.MapProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterUserRequest, User>()
            .ForMember(s => s.Id, map => map.MapFrom(src => Guid.NewGuid()))
            .ForMember(s => s.Role, map => map.MapFrom("User"));
        
        CreateMap<User, UserDto>().ReverseMap();

    }
}