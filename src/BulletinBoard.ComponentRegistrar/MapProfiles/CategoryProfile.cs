using AutoMapper;
using BulletinBoard.Contracts.Categories;
using BulletinBoard.Domain.Categories.Entity;

namespace BulletinBoard.ComponentRegistrar.MapProfiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CreateCategoryRequest, Category>(MemberList.None)
            .ForMember(x => x.Id, map => map.MapFrom(src => Guid.NewGuid()))
            .ForMember(s => s.CreatedAt, map => map.MapFrom(src => DateTime.UtcNow));
        
        CreateMap<Category, CategoryDto>(MemberList.None).ReverseMap();
    }
}