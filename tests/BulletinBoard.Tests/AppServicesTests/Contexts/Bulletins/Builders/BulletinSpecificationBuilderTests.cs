using System.Linq.Expressions;
using AutoFixture;
using BulletinBoard.AppServices.Contexts.Bulletins.Builders;
using BulletinBoard.Contracts.Bulletins;
using BulletinBoard.Domain.Bulletins.Entity;
using Shouldly;

namespace BulletinBoard.Tests.AppServicesTests.Contexts.Bulletins.Builders;

public class BulletinSpecificationBuilderTests
{
    private readonly Fixture _fixture;

    public BulletinSpecificationBuilderTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void Build_WithSearchBulletinRequest_ShouldReturnSpecification()
    {
        // Arrange
        var builder = new BulletinSpecificationBuilder();
        var request = _fixture.Create<SearchBulletinRequest>();
        request.Search = "Test Search";
        request.MinPrice = 10;
        request.MaxPrice = 100;
        request.UserId = Guid.NewGuid();

        // Act
        var specification = builder.Build(request);

        // Assert
        specification.ShouldNotBeNull();

        var testBulletin = new Bulletin
        {
            OwnerId = request.UserId.Value,
            Title = "Test Search",
            Price = 50,
            Description = "Description contains Test Search"
        };

        specification.IsSatisfiedBy(testBulletin).ShouldBeTrue();

        testBulletin.Title = "Other Title";
        specification.IsSatisfiedBy(testBulletin).ShouldBeTrue();

        testBulletin.Title = "Test Search";
        testBulletin.Price = 200;
        specification.IsSatisfiedBy(testBulletin).ShouldBeFalse();
    }

    [Fact]
    public void Build_WithEmptySearchBulletinRequest_ShouldReturnSpecification()
    {
        // Arrange
        var builder = new BulletinSpecificationBuilder();
        var request = _fixture.Create<SearchBulletinRequest>();
        request.Search = string.Empty;
        request.MinPrice = null;
        request.MaxPrice = null;
        request.UserId = null;

        // Act
        var specification = builder.Build(request);

        // Assert
        specification.ShouldNotBeNull();

        var testBulletin = new Bulletin
        {
            OwnerId = Guid.NewGuid(),
        };

        specification.IsSatisfiedBy(testBulletin).ShouldBeTrue();
    }

    [Fact]
    public void Build_WithCategoriesIds_ShouldReturnSpecification()
    {
        // Arrange
        var builder = new BulletinSpecificationBuilder();
        var categoriesIds = _fixture.Create<ICollection<Guid?>>();
        var randomCategoryId = _fixture.Create<Guid?>();

        categoriesIds.Add(randomCategoryId);

        // Act
        var specification = builder.Build(categoriesIds);

        // Assert
        specification.ShouldNotBeNull();

        var testBulletin = new Bulletin
        {
            CategoryId = randomCategoryId.Value
        };

        specification.IsSatisfiedBy(testBulletin).ShouldBeTrue();

        testBulletin.CategoryId = Guid.NewGuid();
        specification.IsSatisfiedBy(testBulletin).ShouldBeFalse();
    }
}