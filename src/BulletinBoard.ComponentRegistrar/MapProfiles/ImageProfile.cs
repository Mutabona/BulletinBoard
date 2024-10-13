using AutoMapper;
using BulletinBoard.Contracts.Files.Images;
using BulletinBoard.Domain.Files.Images.Entity;
using Microsoft.AspNetCore.Http;

namespace BulletinBoard.ComponentRegistrar.MapProfiles;

/// <summary>
/// Профиль изображения.
/// </summary>
public class ImageProfile : Profile
{
    public ImageProfile()
    {
        CreateMap<IFormFile, ImageDto>(MemberList.None)
            .ForMember(s => s.Content, map => map.MapFrom(src => GetBytes(src)))
            .ForMember(s => s.ContentType, map => map.MapFrom(src => src.ContentType))
            .ForMember(s => s.Length, map => map.MapFrom(src => src.Length));
            
        CreateMap<ImageDto, Image>(MemberList.None).ReverseMap();
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