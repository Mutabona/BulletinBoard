using AutoMapper;
using BulletinBoard.Contracts.Categories;
using BulletinBoard.Domain.Categories.Entity;

namespace BulletinBoard.ComponentRegistrar.MapProfiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CreateCategoryRequest, Category>()
            .ForMember(x => x.Id, map => map.MapFrom(src => Guid.NewGuid()));
        
        CreateMap<Category, CategoryDto>().ReverseMap();
    }
}