using AutoFixture;
using BulletinBoard.AppServices.Contexts.Categories.Services;
using BulletinBoard.AppServices.Validators.Bulletins;
using BulletinBoard.Contracts.Bulletins;
using FluentValidation.TestHelper;
using Moq;

namespace BulletinBoard.Tests.AppServicesTests.ValidatorsTests.Bulletins;

/// <summary>
/// Тесты для <see cref="CreateBulletinRequestValidator"/>
/// </summary>
public class CreateBulletinRequestValidatorTests
{
    private readonly CreateBulletinRequestValidator _validator;
    private readonly Mock<ICategoryService> _categoryServiceMock;
    private readonly Fixture _fixture;

    public CreateBulletinRequestValidatorTests()
    {
        _categoryServiceMock = new Mock<ICategoryService>();
        _fixture = new Fixture();
        _validator = new CreateBulletinRequestValidator(_categoryServiceMock.Object);
    }

    /// <summary>
    /// Test valid.
    /// </summary>
    [Fact]
    public void ValidateCreateBulletinRequest_WithValidData_ShouldReturnTrue()
    {
        //Arrange
        var title = _fixture.Create<string>();
        var description = _fixture.Create<string>();
        var price = _fixture.Create<decimal>();
        price *= price; //Чтобы цена всегда была положительна.
        var categoryId = _fixture.Create<Guid>();
        
        var source = _fixture.Build<CreateBulletinRequest>()
            .With(x => x.Title, title)
            .With(x => x.Description, description)
            .With(x => x.Price, price)
            .With(x => x.CategoryId, categoryId)
            .Create();
        
        _categoryServiceMock
            .Setup(x => x.IsCategoryExistsAsync(categoryId, CancellationToken.None)).ReturnsAsync(true);

        //Act
        var result = _validator.TestValidate(source);
        
        //Assert
        _categoryServiceMock.Verify(x => x.IsCategoryExistsAsync(categoryId, CancellationToken.None), Times.Once);
        result.ShouldNotHaveValidationErrorFor(x => x.Title);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Price);
        result.ShouldNotHaveValidationErrorFor(x => x.CategoryId);
    }

    /// <summary>
    /// Test invalid.
    /// </summary>
    [Fact]
    public void ValidateCreateBulletinRequest_WithNullCategoryId_ShouldReturnFalse()
    {
        //Arrange
        var title = _fixture.Create<string>();
        var description = _fixture.Create<string>();
        var price = _fixture.Create<decimal>();
        price *= price; //Чтобы цена всегда была положительна.
        Guid? categoryId = null;
        
        var source = _fixture.Build<CreateBulletinRequest>()
            .With(x => x.Title, title)
            .With(x => x.Description, description)
            .With(x => x.Price, price)
            .With(x => x.CategoryId, categoryId)
            .Create();
        
        //Act
        var result = _validator.TestValidate(source);

        //Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Title);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Price);
        result.ShouldHaveValidationErrorFor(x => x.CategoryId).WithErrorMessage("Категория не может быть пустой.");
    }
    
    /// <summary>
    /// Test invalid.
    /// </summary>
    [Fact]
    public void ValidateCreateBulletinRequest_WithInvalidCategoryId_ShouldReturnFalse()
    {
        //Arrange
        var title = _fixture.Create<string>();
        var description = _fixture.Create<string>();
        var price = _fixture.Create<decimal>();
        price *= price; //Чтобы цена всегда была положительна.
        var categoryId = _fixture.Create<Guid>();
        
        var source = _fixture.Build<CreateBulletinRequest>()
            .With(x => x.Title, title)
            .With(x => x.Description, description)
            .With(x => x.Price, price)
            .With(x => x.CategoryId, categoryId)
            .Create();
        
        _categoryServiceMock
            .Setup(x => x.IsCategoryExistsAsync(categoryId, CancellationToken.None)).ReturnsAsync(false);
        
        //Act
        var result = _validator.TestValidate(source);

        //Assert
        _categoryServiceMock.Verify(x => x.IsCategoryExistsAsync(categoryId, CancellationToken.None), Times.Once);
        result.ShouldNotHaveValidationErrorFor(x => x.Title);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Price);
        result.ShouldHaveValidationErrorFor(x => x.CategoryId).WithErrorMessage("Неверная категория.");
    }

    /// <summary>
    /// Test invalid.
    /// </summary>
    [Fact]
    public void ValidateCreateBulletinRequest_WithInvalidTitle_ShouldReturnFalse()
    {
        //Arrange
        var title = "";
        string? description = null;
        var price = _fixture.Create<decimal>();
        price *= price; //Чтобы цена всегда была положительна.
        var categoryId = _fixture.Create<Guid>();
        
        var source = _fixture.Build<CreateBulletinRequest>()
            .With(x => x.Title, title)
            .With(x => x.Description, description)
            .With(x => x.Price, price)
            .With(x => x.CategoryId, categoryId)
            .Create();
        
        _categoryServiceMock
            .Setup(x => x.IsCategoryExistsAsync(categoryId, CancellationToken.None)).ReturnsAsync(true);
        
        //Act
        var result = _validator.TestValidate(source);

        //Assert
        _categoryServiceMock.Verify(x => x.IsCategoryExistsAsync(categoryId, CancellationToken.None), Times.Once);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Price);
        result.ShouldNotHaveValidationErrorFor(x => x.CategoryId);
        result.ShouldHaveValidationErrorFor(x => x.Title).WithErrorMessage("Название не может быть пустым.");
    }

    /// <summary>
    /// Test invalid.
    /// </summary>
    [Fact]
    public void ValidateCreateBulletinRequest_WithInvalidPrice_ShouldReturnFalse()
    {
        //Arrange
        var title = _fixture.Create<string>();
        string? description = null;
        var price = -1;
        var categoryId = _fixture.Create<Guid>();
        
        var source = _fixture.Build<CreateBulletinRequest>()
            .With(x => x.Title, title)
            .With(x => x.Description, description)
            .With(x => x.Price, price)
            .With(x => x.CategoryId, categoryId)
            .Create();
        
        _categoryServiceMock
            .Setup(x => x.IsCategoryExistsAsync(categoryId, CancellationToken.None)).ReturnsAsync(true);
        
        //Act
        var result = _validator.TestValidate(source);

        //Assert
        _categoryServiceMock.Verify(x => x.IsCategoryExistsAsync(categoryId, CancellationToken.None), Times.Once);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.CategoryId);
        result.ShouldNotHaveValidationErrorFor(x => x.Title);
        result.ShouldHaveValidationErrorFor(x => x.Price).WithErrorMessage("Цена не может быть меньше нуля.");
    }

    /// <summary>
    /// Test invalid.
    /// </summary>
    [Fact]
    public void ValidateCreateBulletinRequest_WithNullPrice_ShouldReturnFalse()
    {
        //Arrange
        var title = _fixture.Create<string>();
        string? description = null;
        decimal? price = null;
        var categoryId = _fixture.Create<Guid>();
        
        //Act
        var source = _fixture.Build<CreateBulletinRequest>()
            .With(x => x.Title, title)
            .With(x => x.Description, description)
            .With(x => x.Price, price)
            .With(x => x.CategoryId, categoryId)
            .Create();
        
        _categoryServiceMock
            .Setup(x => x.IsCategoryExistsAsync(categoryId, CancellationToken.None)).ReturnsAsync(true);
        
        var result = _validator.TestValidate(source);

        //Assert
        _categoryServiceMock.Verify(x => x.IsCategoryExistsAsync(categoryId, CancellationToken.None), Times.Once);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.CategoryId);
        result.ShouldNotHaveValidationErrorFor(x => x.Title);
        result.ShouldHaveValidationErrorFor(x => x.Price).WithErrorMessage("Цена должна быть заполнена.");
    }
}