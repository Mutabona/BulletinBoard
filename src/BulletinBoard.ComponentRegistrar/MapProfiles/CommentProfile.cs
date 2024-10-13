using AutoMapper;
using BulletinBoard.Contracts.Comments;
using BulletinBoard.Domain.Comments.Entity;

namespace BulletinBoard.ComponentRegistrar.MapProfiles;

/// <summary>
/// Профиль комментария.
/// </summary>
public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<AddCommentRequest, CommentDto>(MemberList.None);

        CreateMap<CommentDto, Comment>(MemberList.None);
        
        CreateMap<Comment, CommentDto>()
            .ForMember(x => x.AuthorName, map => map.MapFrom(src => src.Author.Name));
    }
}