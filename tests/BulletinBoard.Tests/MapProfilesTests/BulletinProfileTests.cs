using AutoFixture;
using AutoMapper;
using BulletinBoard.ComponentRegistrar.MapProfiles;
using BulletinBoard.Contracts.Bulletins;
using BulletinBoard.Domain.Bulletins.Entity;
using BulletinBoard.Domain.Categories.Entity;
using BulletinBoard.Domain.Comments.Entity;
using BulletinBoard.Domain.Files.Images.Entity;
using BulletinBoard.Domain.Users.Entity;
using FluentAssertions;
using Microsoft.Extensions.Time.Testing;

namespace BulletinBoard.Tests.MapProfilesTests;

/// <summary>
/// Тесты для <see cref="BulletinProfile"/>
/// </summary>
public class BulletinProfileTests
{
    private readonly MapperConfiguration _configurationProfider;
    private readonly IMapper _mapper;
    private readonly Fixture _fixture;
    private readonly FakeTimeProvider _timeProvider;
    
    public BulletinProfileTests()
    {
        _timeProvider = new FakeTimeProvider();
        _timeProvider.SetUtcNow(DateTime.UtcNow);
        _configurationProfider = new MapperConfiguration(delegate(IMapperConfigurationExpression configure)
        {
            configure.AddProfile(new BulletinProfile());
        });
        _fixture = new Fixture();
        _mapper = _configurationProfider.CreateMapper();
    }

    [Fact]
    public void BulletinProfile_CheckConfigurationIsValid()
    {
        _configurationProfider.AssertConfigurationIsValid();
    }

    [Fact]
    public void BulletinProfile_CreateBulletinRequest_To_BulletinDto()
    {
        //Arrange
        var title = _fixture.Create<string>();
        var description = _fixture.Create<string>();
        var categoryId = _fixture.Create<Guid>();
        var price = _fixture.Create<decimal>();
        price *= price; //Чтобы цена была всегда положительна.
        var createdAt = _timeProvider.GetUtcNow().DateTime;
        
        var source = _fixture
            .Build<CreateBulletinRequest>()
            .With(x => x.Title, title)
            .With(x => x.Description, description)
            .With(x => x.Price, price)
            .With(x => x.CategoryId, categoryId)
            .Create();
        
        //Act
        var result = _mapper.Map<CreateBulletinRequest, BulletinDto>(source);
        
        //Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(
            new
            {
                title,
                description,
                price,
                categoryId,
                createdAt
            },
            opt => opt.ExcludingMissingMembers());
    }
    
    [Fact]
    public void BulletinProfile_Bulletin_To_BulletinDto()
    {
        //Arrange
        var id = _fixture.Create<Guid>();
        var title = _fixture.Create<string>();
        var description = _fixture.Create<string>();
        var price = _fixture.Create<decimal>();
        var createdAt = _fixture.Create<DateTime>();
        price *= price; //Чтобы цена была всегда положительна.
        
        var categoryId = _fixture.Create<Guid>();
        var categoryName = _fixture.Create<string>();
        var category = new Category()
        {
            Id = categoryId,
            Name = categoryName,
        };
        
        var ownerId = _fixture.Create<Guid>();
        var ownerName = _fixture.Create<string>();
        var owner = new User()
        {
            Id = ownerId,
            Name = ownerName,
        };
        
        
        var source = _fixture
            .Build<Bulletin>()
            .With(x => x.Id, id)
            .With(x => x.CreatedAt, createdAt)
            .With(x => x.OwnerId, ownerId)
            .With(x => x.Owner, owner)
            .With(x => x.Title, title)
            .With(x => x.Description, description)
            .With(x => x.CategoryId, categoryId)
            .With(x => x.Category, category)
            .With(x => x.Price, price)
            .With(x => x.Images, new List<Image>())
            .With(x => x.Comments, new List<Comment>())
            .Create();
        
        //Act
        var result = _mapper.Map<Bulletin, BulletinDto>(source);
        
        //Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(
            new
            {
                id,
                createdAt,
                ownerId,
                ownerName,
                title,
                description,
                categoryId,
                category,
                categoryName,
                price,
            },
            opt => opt.ExcludingMissingMembers());
    }

    [Fact]
    public void BulletinProfile_BulletinDto_To_Bulletin()
    {
        //Arrange
        var id = _fixture.Create<Guid>();
        var title = _fixture.Create<string>();
        var description = _fixture.Create<string>();
        var price = _fixture.Create<decimal>();
        var createdAt = _fixture.Create<DateTime>();
        var categoryId = _fixture.Create<Guid>();
        var ownerId = _fixture.Create<Guid>();
        price *= price; //Чтобы цена была всегда положительна.
        
        var source = _fixture
            .Build<BulletinDto>()
            .With(x => x.Id, id)
            .With(x => x.CreatedAt, createdAt)
            .With(x => x.OwnerId, ownerId)
            .With(x => x.Title, title)
            .With(x => x.Description, description)
            .With(x => x.CategoryId, categoryId)
            .With(x => x.Price, price)
            .Create();
        
        //Act
        var result = _mapper.Map<BulletinDto, Bulletin>(source);
        
        //Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(
            new
            {
                id,
                createdAt,
                ownerId,
                title,
                description,
                categoryId,
                price,
            },
            opt => opt.ExcludingMissingMembers());
    }
}