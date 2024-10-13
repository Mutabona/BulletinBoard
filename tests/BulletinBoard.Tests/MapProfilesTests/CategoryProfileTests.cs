using AutoFixture;
using AutoMapper;
using BulletinBoard.ComponentRegistrar.MapProfiles;
using BulletinBoard.Contracts.Categories;
using BulletinBoard.Domain.Bulletins.Entity;
using BulletinBoard.Domain.Categories.Entity;
using FluentAssertions;
using Microsoft.Extensions.Time.Testing;

namespace BulletinBoard.Tests.MapProfilesTests;

/// <summary>
/// Тесты для <see cref="CategoryProfile"/>.
/// </summary>
public class CategoryProfileTests
{
    private readonly MapperConfiguration _configurationProfider;
    private readonly IMapper _mapper;
    private readonly Fixture _fixture;
    private readonly FakeTimeProvider _timeProvider;
    
    public CategoryProfileTests()
    {
        _timeProvider = new FakeTimeProvider();
        _timeProvider.SetUtcNow(DateTimeOffset.UtcNow);
        _configurationProfider = new MapperConfiguration(delegate(IMapperConfigurationExpression configure)
        {
            configure.AddProfile(new CategoryProfile());
        });
        _fixture = new Fixture();
        _mapper = _configurationProfider.CreateMapper();
    }

    [Fact]
    public void CategoryProfile_CheckConfigurationIsValid()
    {
        _configurationProfider.AssertConfigurationIsValid();
    }

    [Fact]
    public void CategoryProfile_CreateCategoryRequest_To_Category()
    {
        //Arrange
        var name = _fixture.Create<string>();
        var parentCategoryId = _fixture.Create<Guid>();
        var createdAt = _timeProvider.GetUtcNow().DateTime;
        
        var source = _fixture
            .Build<CreateCategoryRequest>()
            .With(x => x.Name, name)
            .With(x => x.ParentCategoryId, parentCategoryId)
            .Create();
        
        //Act
        var result = _mapper.Map<CategoryDto>(source);
        
        //Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(
            new
            {
                name,
                parentCategoryId,
                createdAt
            },
            opt => opt.ExcludingMissingMembers());
    }

    [Fact]
    public void CategoryProfile_Category_To_CategoryDto()
    {
        //Arrange
        var id = _fixture.Create<Guid>();
        var createdAt = _fixture.Create<DateTime>();
        var name = _fixture.Create<string>();
        var parentCategoryId = _fixture.Create<Guid>();
        
        var source = _fixture
            .Build<Category>()
            .With(x => x.Id, id)
            .With(x => x.CreatedAt, createdAt)
            .With(x => x.Name, name)
            .With(x => x.ParentCategoryId, parentCategoryId)
            .With(x => x.ParentCategory, new Category())
            .With(x => x.Subcategories, new List<Category>())
            .With(x => x.Bulletins, new List<Bulletin>())
            .Create();
        
        //Act
        var result = _mapper.Map<CategoryDto>(source);
        
        //Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(
            new
            {
                id,
                createdAt,
                name,
                parentCategoryId,
            },
            opt => opt.ExcludingMissingMembers());
    }
    
    [Fact]
    public void CategoryProfile_CategoryDto_To_Category()
    {
        //Arrange
        var id = _fixture.Create<Guid>();
        var createdAt = _fixture.Create<DateTime>();
        var name = _fixture.Create<string>();
        var parentCategoryId = _fixture.Create<Guid>();
        
        var source = _fixture
            .Build<Category>()
            .With(x => x.Id, id)
            .With(x => x.CreatedAt, createdAt)
            .With(x => x.Name, name)
            .With(x => x.ParentCategoryId, parentCategoryId)
            .With(x => x.ParentCategory, new Category())
            .With(x => x.Subcategories, new List<Category>())
            .With(x => x.Bulletins, new List<Bulletin>())
            .Create();
        
        //Act
        var result = _mapper.Map<CategoryDto>(source);
        
        //Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(
            new
            {
                id,
                createdAt,
                name,
                parentCategoryId,
            },
            opt => opt.ExcludingMissingMembers());
    }
}