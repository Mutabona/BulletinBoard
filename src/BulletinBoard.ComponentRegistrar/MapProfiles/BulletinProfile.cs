using AutoMapper;
using BulletinBoard.Contracts.Bulletins;
using BulletinBoard.Domain.Bulletins.Entity;

namespace BulletinBoard.ComponentRegistrar.MapProfiles;

/// <summary>
/// Профиль объявления.
/// </summary>
public class BulletinProfile : Profile
{
    public BulletinProfile()
    {
        CreateMap<CreateBulletinRequest, BulletinDto>(MemberList.None);

        CreateMap<BulletinDto, Bulletin>(MemberList.None);

        CreateMap<Bulletin, BulletinDto>(MemberList.None)
            .ForMember(s => s.CategoryName, map => map.MapFrom(src => src.Category.Name))
            .ForMember(s => s.CategoryId, map => map.MapFrom(src => src.CategoryId))
            .ForMember(s => s.OwnerName, map => map.MapFrom(src => src.Owner.Name));
    }
}