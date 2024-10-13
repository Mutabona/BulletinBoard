using AutoFixture;
using AutoMapper;
using BulletinBoard.ComponentRegistrar.MapProfiles;
using BulletinBoard.Contracts.Files.Images;
using BulletinBoard.Domain.Bulletins.Entity;
using BulletinBoard.Domain.Files.Images.Entity;
using FluentAssertions;
using Microsoft.Extensions.Time.Testing;

namespace BulletinBoard.Tests.MapProfilesTests;

/// <summary>
/// Тесты для <see cref="ImageProfile"/>
/// </summary>
public class ImageProfileTests
{
    private readonly MapperConfiguration _configurationProfider;
    private readonly IMapper _mapper;
    private readonly Fixture _fixture;
    private readonly FakeTimeProvider _timeProvider;
    
    public ImageProfileTests()
    {
        _timeProvider = new FakeTimeProvider();
        _timeProvider.SetUtcNow(DateTime.UtcNow);
        _configurationProfider = new MapperConfiguration(delegate(IMapperConfigurationExpression configure)
        {
            configure.AddProfile(new ImageProfile());
        });
        _fixture = new Fixture();
        _mapper = _configurationProfider.CreateMapper();
    }

    [Fact]
    public void ImageProfile_CheckConfigurationIsValid()
    {
        _configurationProfider.AssertConfigurationIsValid();
    }

    [Fact]
    public void ImageProfile_Image_To_ImageDto()
    {
        //Arrange
        var id = _fixture.Create<Guid>();
        var createdAt = _fixture.Create<DateTime>();
        var content = _fixture.Create<byte[]>();
        var contentType = "image/jpeg";
        var length = content.Length;
        var bulletinId = _fixture.Create<Guid>();
        var bulletin = new Bulletin()
        {
            Id = bulletinId,
        };
        
        var source = _fixture
            .Build<Image>()
            .With(x => x.Id, id)
            .With(x => x.BulletinId, bulletinId)
            .With(x => x.CreatedAt, createdAt)
            .With(x => x.ContentType, contentType)
            .With(x => x.Content, content)
            .With(x => x.Length, length)
            .With(x => x.Bulletin, bulletin)
            .Create();
        
        //Act
        var result = _mapper.Map<ImageDto>(source);
        
        //Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new
        {
            id,
            bulletinId,
            content,
            contentType,
            length,
        },
            opt => opt.ExcludingMissingMembers());
    }
    
    [Fact]
    public void ImageProfile_ImageDto_To_Image()
    {
        //Arrange
        var id = _fixture.Create<Guid>();
        var createdAt = _timeProvider.GetUtcNow().DateTime;
        var content = _fixture.Create<byte[]>();
        var contentType = "image/jpeg";
        var length = content.Length;
        var bulletinId = _fixture.Create<Guid>();
        
        var source = _fixture
            .Build<ImageDto>()
            .With(x => x.Id, id)
            .With(x => x.CreatedAt, createdAt)
            .With(x => x.BulletinId, bulletinId)
            .With(x => x.ContentType, contentType)
            .With(x => x.Content, content)
            .With(x => x.Length, length)
            .Create();
        
        //Act
        var result = _mapper.Map<ImageDto>(source);
        
        //Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new
            {
                id,
                createdAt,
                bulletinId,
                content,
                contentType,
                length,
            },
            opt => opt.ExcludingMissingMembers());
    }
}