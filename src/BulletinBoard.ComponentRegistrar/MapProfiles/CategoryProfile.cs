using AutoMapper;
using BulletinBoard.Contracts.Categories;
using BulletinBoard.Domain.Categories.Entity;

namespace BulletinBoard.ComponentRegistrar.MapProfiles;

/// <summary>
/// Профиль категории.
/// </summary>
public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CreateCategoryRequest, CategoryDto>(MemberList.None);
        
        CreateMap<Category, CategoryDto>(MemberList.None).ReverseMap();
    }
}