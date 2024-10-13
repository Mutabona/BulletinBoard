using AutoFixture;
using AutoMapper;
using BulletinBoard.ComponentRegistrar.MapProfiles;
using BulletinBoard.Contracts.Comments;
using BulletinBoard.Domain.Bulletins.Entity;
using BulletinBoard.Domain.Comments.Entity;
using BulletinBoard.Domain.Users.Entity;
using FluentAssertions;
using Microsoft.Extensions.Time.Testing;

namespace BulletinBoard.Tests.MapProfilesTests;

/// <summary>
/// Тесты для <see cref="CommentProfile"/>
/// </summary>
public class CommentProfileTests
{
    private readonly MapperConfiguration _configurationProfider;
    private readonly IMapper _mapper;
    private readonly Fixture _fixture;
    private readonly FakeTimeProvider _timeProvider;
    
    public CommentProfileTests()
    {
        _timeProvider = new FakeTimeProvider();
        _timeProvider.SetUtcNow(DateTimeOffset.UtcNow);
        _configurationProfider = new MapperConfiguration(delegate(IMapperConfigurationExpression configure)
        {
            configure.AddProfile(new CommentProfile());
        });
        _fixture = new Fixture();
        _mapper = _configurationProfider.CreateMapper();
    }

    [Fact]
    public void CommentProfile_CheckConfigurationIsValid()
    {
        _configurationProfider.AssertConfigurationIsValid();
    }

    [Fact]
    public void CommentProfile_AddCommentRequest_To_CommentDto()
    {
        //Arrange
        var text = _fixture.Create<string>();
        
        var source = _fixture
            .Build<AddCommentRequest>()
            .With(x => x.Text, text)
            .Create();
        
        //Act
        var result = _mapper.Map<CommentDto>(source);
        
        //Assert
        result.Should().NotBeNull();
        result.Text.Should().Be(text);
    }

    [Fact]
    public void CommentProfile_Comment_To_CommentDto()
    {
        //Arrange
        var id = _fixture.Create<Guid>();
        var authorId = _fixture.Create<Guid>();
        var authorName = _fixture.Create<string>();
        var createdAt = _fixture.Create<DateTime>();
        var author = new User()
        {
            Id = authorId,
            Name = authorName
        };
        var bulletinId = _fixture.Create<Guid>();
        var bulletin = new Bulletin()
        {
            Id = bulletinId,
        };
        var text = _fixture.Create<string>();
        
        var source = _fixture
            .Build<Comment>()
            .With(x => x.Id, id)
            .With(x => x.CreatedAt, createdAt)
            .With(x => x.AuthorId, authorId)
            .With(x => x.BulletinId, bulletinId)
            .With(x => x.Text, text)
            .With(x => x.Author, author)
            .With(x => x.Bulletin, bulletin)
            .Create();
        
        //Act
        var result = _mapper.Map<CommentDto>(source);
        
        //Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new
        {
            id,
            createdAt,
            authorId,
            authorName,
            bulletinId,
            text
        },
            opt => opt.ExcludingMissingMembers());
    }

    [Fact]
    public void CommentProfile_CommentDto_To_Comment()
    {
        //Arrange
        var id = _fixture.Create<Guid>();
        var createdAt = _fixture.Create<DateTime>();
        var authorId = _fixture.Create<Guid>();
        var bulletinId = _fixture.Create<Guid>();
        var text = _fixture.Create<string>();
        
        var source = _fixture
            .Build<CommentDto>()
            .With(x => x.Id, id)
            .With(x => x.CreatedAt, createdAt)
            .With(x => x.AuthorId, authorId)
            .With(x => x.BulletinId, bulletinId)
            .With(x => x.Text, text)
            .Create();
        
        //Act
        var result = _mapper.Map<Comment>(source);
        
        //Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new
            {
                id,
                createdAt,
                authorId,
                bulletinId,
                text
            },
            opt => opt.ExcludingMissingMembers());
    }
}