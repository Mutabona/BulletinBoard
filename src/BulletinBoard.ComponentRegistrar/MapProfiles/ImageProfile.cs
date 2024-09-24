using AutoMapper;
using BulletinBoard.Contracts.Files.Images;
using BulletinBoard.Domain.Files.Images.Entity;
using Microsoft.AspNetCore.Http;

namespace BulletinBoard.ComponentRegistrar.MapProfiles;

public class ImageProfile : Profile
{
    public ImageProfile()
    {
        CreateMap<AddImageRequest, Image>(MemberList.None)
            .ForMember(s => s.Id, map => map.MapFrom(src => Guid.NewGuid()))
            .ForMember(s => s.Content, map => map.MapFrom(src => GetBytes(src.Image)))
            .ForMember(s => s.ContentType, map => map.MapFrom(src => src.Image.ContentType))
            .ForMember(s => s.Length, map => map.MapFrom(src => src.Image.Length))
            .ForMember(s => s.CreatedAt, map => map.MapFrom(src => DateTime.UtcNow));
            
        
        CreateMap<Image, ImageDto>(MemberList.None).ReverseMap();
    }
    
    /// <summary>
    /// Возвращает набор байт из файла.
    /// </summary>
    /// <param name="file">Файл <see cref="IFormFile"/></param>
    /// <returns>Набор байт.</returns>
    public byte[] GetBytes(IFormFile file)
    {
        var ms = new MemoryStream();
        file.CopyTo(ms);
        return ms.ToArray();
    }
}