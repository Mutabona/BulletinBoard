using AutoFixture;
using BulletinBoard.AppServices.Contexts.Categories.Services;
using BulletinBoard.AppServices.Validators.Categories;
using BulletinBoard.Contracts.Categories;
using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;

namespace BulletinBoard.Tests.AppServicesTests.ValidatorsTests.Categories;

/// <summary>
/// Тесты для <see cref="CreateCategoryValidator"/>
/// </summary>
public class CreateCategoryValidatorTests
{
    private readonly CreateCategoryValidator _validator;
    private readonly Mock<ICategoryService> _categoryServiceMock;
    private readonly Fixture _fixture;

    public CreateCategoryValidatorTests()
    {
        _fixture = new Fixture();
        _categoryServiceMock = new Mock<ICategoryService>();
        _validator = new CreateCategoryValidator(_categoryServiceMock.Object);
    }

    /// <summary>
    /// Test valid.
    /// </summary>
    [Fact]
    public void Validate_ValidCreateCategory_ShouldReturnTrue()
    {
        var name = _fixture.Create<string>();
        var parentCategoryId = _fixture.Create<Guid>();

        var source = _fixture.Build<CreateCategoryRequest>()
            .With(x => x.Name, name)
            .With(x => x.ParentCategoryId, parentCategoryId)
            .Create();

        _categoryServiceMock
            .Setup(x => x.IsCategoryExistsAsync(parentCategoryId, CancellationToken.None)).ReturnsAsync(true);


        var result = _validator.TestValidate(source);

        _categoryServiceMock.Verify(x => x.IsCategoryExistsAsync(parentCategoryId, CancellationToken.None), Times.Once);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.ParentCategoryId);
        result.IsValid.Should().BeTrue();
    }

    /// <summary>
    /// Test invalid.
    /// </summary>
    [Fact]
    public void Validate_InvalidParentCategoryId_ShouldReturnFalse()
    {
        var name = _fixture.Create<string>();
        var parentCategoryId = _fixture.Create<Guid>();
        
        var source = _fixture.Build<CreateCategoryRequest>()
            .With(x => x.Name, name)
            .With(x => x.ParentCategoryId, parentCategoryId)
            .Create();
        
        _categoryServiceMock
            .Setup(x => x.IsCategoryExistsAsync(parentCategoryId, CancellationToken.None)).ReturnsAsync(false);
        
        var result = _validator.TestValidate(source);

        _categoryServiceMock.Verify(x => x.IsCategoryExistsAsync(parentCategoryId, CancellationToken.None), Times.Once);
        result.ShouldHaveValidationErrorFor(x => x.ParentCategoryId).WithErrorMessage("Неверная родительская категория.");
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.IsValid.Should().BeFalse();
    }

    /// <summary>
    /// Test invalid.
    /// </summary>
    [Fact]
    public void Validate_InvalidName_ShouldReturnFalse()
    {
        var name = "";
        Guid? parentCategoryId = null;
        
        var source = _fixture.Build<CreateCategoryRequest>()
            .With(x => x.Name, name)
            .With(x => x.ParentCategoryId, parentCategoryId)
            .Create();
        
        var result = _validator.TestValidate(source);
        
        //_categoryServiceMock.Verify(x => x.IsCategoryExistsAsync(parentCategoryId.Value, CancellationToken.None), Times.Never);
        result.ShouldHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.ParentCategoryId);
        result.IsValid.Should().BeFalse();
    }
}