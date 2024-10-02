using AutoMapper;
using BulletinBoard.Contracts.Comments;
using BulletinBoard.Domain.Comments.Entity;

namespace BulletinBoard.ComponentRegistrar.MapProfiles;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<AddCommentRequest, Comment>(MemberList.None)
            .ForMember(x => x.Id, map => map.MapFrom(src => Guid.NewGuid()))
            .ForMember(x => x.CreatedAt, map => map.MapFrom(src => DateTime.UtcNow));
        
        CreateMap<Comment, CommentDto>();
    }
}