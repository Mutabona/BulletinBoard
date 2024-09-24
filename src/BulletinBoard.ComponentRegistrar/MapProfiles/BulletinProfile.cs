﻿using AutoMapper;
using BulletinBoard.Contracts.Bulletins;
using BulletinBoard.Domain.Bulletins.Entity;

namespace BulletinBoard.ComponentRegistrar.MapProfiles;

public class BulletinProfile : Profile
{
    public BulletinProfile()
    {
        CreateMap<CreateBulletinRequest, Bulletin>(MemberList.None)
            .ForMember(s => s.Id, map => map.MapFrom(src => Guid.NewGuid()))
            .ForMember(s => s.CreatedAt, map => map.MapFrom(src => DateTime.UtcNow));
        
        CreateMap<Bulletin, BulletinDto>(MemberList.None)
            .ForMember(s => s.CategoryName, map => map.MapFrom(src => src.Category.Name))
            .ForMember(s => s.CategoryId, map => map.MapFrom(src => src.Category.Id))
            .ForMember(s => s.OwnerName, map => map.MapFrom(src => src.Owner.Name))
            .ForMember(s => s.OwnerLastname, map => map.MapFrom(src => src.Owner.Lastname))
            .ForMember(s => s.OwnerSurname, map => map.MapFrom(src => src.Owner.Surname));

        CreateMap<BulletinDto, Bulletin>(MemberList.None);
    }
}