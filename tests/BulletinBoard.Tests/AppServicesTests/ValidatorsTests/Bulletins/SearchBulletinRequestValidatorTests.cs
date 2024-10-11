using AutoFixture;
using BulletinBoard.AppServices.Contexts.Users.Services;
using BulletinBoard.AppServices.Exceptions;
using BulletinBoard.AppServices.Validators.Bulletins;
using BulletinBoard.Contracts.Bulletins;
using Moq;
using Shouldly;

namespace BulletinBoard.Tests.AppServicesTests.ValidatorsTests.Bulletins;

/// <summary>
/// Тесты для <see cref="SearchBulletinRequestValidator"/>
/// </summary>
public class SearchBulletinRequestValidatorTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IUserService> _userServiceMock;
    private readonly SearchBulletinRequestValidator _validator;

    public SearchBulletinRequestValidatorTests()
    {
        _fixture = new Fixture();
        _userServiceMock = _fixture.Freeze<Mock<IUserService>>();
        _validator = new SearchBulletinRequestValidator(_userServiceMock.Object);
    }

    /// <summary>
    /// Test invalid.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_Search_Exceeds_MaxLength()
    {
        // Arrange
        var request = _fixture.Build<SearchBulletinRequest>()
                              .With(x => x.Search, new string('a', 1501))
                              .Create();

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.Errors.ShouldContain(x => x.PropertyName == "Search" && x.ErrorMessage == "Превышена максимальная длинна текста поиска.");
    }

    /// <summary>
    /// Test invalid.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_MaxPrice_Is_Less_Than_Zero()
    {
        // Arrange
        var request = _fixture.Build<SearchBulletinRequest>()
                              .With(x => x.MaxPrice, -1)
                              .Create();

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.Errors.ShouldContain(x => x.PropertyName == "MaxPrice" && x.ErrorMessage == "Максимальная цена не может быть меньше нуля.");
    }

    /// <summary>
    /// Test invalid.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_MinPrice_Is_Less_Than_Zero()
    {
        // Arrange
        var request = _fixture.Build<SearchBulletinRequest>()
                              .With(x => x.MinPrice, -1)
                              .Create();

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.Errors.ShouldContain(x => x.PropertyName == "MinPrice" && x.ErrorMessage == "Минимальная цена не может быть меньше нуля.");
    }

    /// <summary>
    /// Test invalid.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_Take_Is_Less_Than_Zero()
    {
        // Arrange
        var request = _fixture.Build<SearchBulletinRequest>()
                              .With(x => x.Take, -1)
                              .Create();

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.Errors.ShouldContain(x => x.PropertyName == "Take");
    }

    /// <summary>
    /// Test invalid.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_Skip_Is_Less_Than_Zero()
    {
        // Arrange
        var request = _fixture.Build<SearchBulletinRequest>()
                              .With(x => x.Skip, -1)
                              .Create();

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.Errors.ShouldContain(x => x.PropertyName == "Skip");
    }

    /// <summary>
    /// Test invalid.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_UserId_Does_Not_Exist()
    {
        // Arrange
        var userId = _fixture.Create<Guid>();
        _userServiceMock.Setup(x => x.GetUserByIdAsync(userId, It.IsAny<CancellationToken>())).ThrowsAsync(new EntityNotFoundException());
        
        var request = _fixture.Build<SearchBulletinRequest>()
                              .With(x => x.UserId, userId)
                              .Create();

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.Errors.ShouldContain(x => x.PropertyName == "UserId" && x.ErrorMessage == "Такого пользователя не существует.");
    }

    /// <summary>
    /// Test valid.
    /// </summary>
    [Fact]
    public void Should_Not_Have_Error_When_UserId_Is_Null()
    {
        // Arrange
        var request = _fixture.Build<SearchBulletinRequest>()
                              .With(x => x.UserId, (Guid?)null)
                              .Create();

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.Errors.ShouldBeEmpty();
    }
}