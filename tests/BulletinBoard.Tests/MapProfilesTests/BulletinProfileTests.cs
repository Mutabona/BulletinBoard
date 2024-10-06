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

namespace BulletinBoard.Tests.MapProfilesTests;

/// <summary>
/// Тесты для <see cref="BulletinProfile"/>
/// </summary>
public class BulletinProfileTests
{
    private readonly MapperConfiguration _configurationProfider;
    private readonly IMapper _mapper;
    private readonly Fixture _fixture;
    
    public BulletinProfileTests()
    {
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
    public void BulletinProfile_CreateBulletinRequest_To_Bulletin()
    {
        var title = _fixture.Create<string>();
        var description = _fixture.Create<string>();
        var categoryId = _fixture.Create<Guid>();
        var price = _fixture.Create<decimal>();
        price *= price; //Чтобы цена была всегда положительна.
        
        var source = _fixture
            .Build<CreateBulletinRequest>()
            .With(x => x.Title, title)
            .With(x => x.Description, description)
            .With(x => x.Price, price)
            .With(x => x.CategoryId, categoryId)
            .Create();
        
        var result = _mapper.Map<CreateBulletinRequest, Bulletin>(source);
        
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(
            new
            {
                title,
                description,
                price,
                categoryId,
            },
            opt => opt.ExcludingMissingMembers());
    }
    
    [Fact]
    public void BulletinProfile_Bulletin_To_BulletinDto()
    {
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
        
        var result = _mapper.Map<Bulletin, BulletinDto>(source);
        
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
                price,
            },
            opt => opt.ExcludingMissingMembers());
    }
}