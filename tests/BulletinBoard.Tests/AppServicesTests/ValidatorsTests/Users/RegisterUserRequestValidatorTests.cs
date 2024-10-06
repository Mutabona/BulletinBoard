using AutoFixture;
using BulletinBoard.AppServices.Contexts.Users.Services;
using BulletinBoard.AppServices.Validators.Users;
using BulletinBoard.Contracts.Users;
using FluentValidation.TestHelper;
using Moq;

namespace BulletinBoard.Tests.AppServicesTests.ValidatorsTests.Users;

/// <summary>
/// Тесты для <see cref="RegisterUserRequestValidator"/>.
/// </summary>
public class RegisterUserRequestValidatorTests
{
    private readonly RegisterUserRequestValidator _validator;
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Fixture _fixture;

    public RegisterUserRequestValidatorTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _validator = new RegisterUserRequestValidator(_userServiceMock.Object);
        _fixture = new Fixture();
    }

    /// <summary>
    /// Test valid.
    /// </summary>
    [Fact]
    public void ValidateRegisterUserRequest_ValidData_ShouldReturnTrue()
    {
        var password = _fixture.Create<string>();
        var email = "test@email.com";
        var name = _fixture.Create<string>();
        var phone = "+79781195738";
        
        var source = _fixture
            .Build<RegisterUserRequest>()
            .With(x => x.Email, email)
            .With(x => x.Name, name)
            .With(x => x.Phone, phone)
            .With(x => x.Password, password)
            .Create();
        
        _userServiceMock.Setup(x => x.IsUniqueEmailAsync(email, CancellationToken.None)).ReturnsAsync(true);
        
        var result = _validator.TestValidate(source);
        
        _userServiceMock.Verify(x => x.IsUniqueEmailAsync(email, CancellationToken.None), Times.Once);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Phone);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }

    /// <summary>
    /// Test invalid.
    /// </summary>
    [Fact]
    public void ValidateRegisterUserRequest_EmptyPassword_ShouldReturnFalse()
    {
        var password = "";
        var email = "test@email.com";
        var name = _fixture.Create<string>();
        var phone = "+79781195738";
        
        var source = _fixture
            .Build<RegisterUserRequest>()
            .With(x => x.Email, email)
            .With(x => x.Name, name)
            .With(x => x.Phone, phone)
            .With(x => x.Password, password)
            .Create();
        
        _userServiceMock.Setup(x => x.IsUniqueEmailAsync(email, CancellationToken.None)).ReturnsAsync(true);
        
        var result = _validator.TestValidate(source);
        
        _userServiceMock.Verify(x => x.IsUniqueEmailAsync(email, CancellationToken.None), Times.Once);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Phone);
        result.ShouldHaveValidationErrorFor(x => x.Password).WithErrorMessage("Пароль не может быть пустым.");
    }
    
    /// <summary>
    /// Test invalid.
    /// </summary>
    [Fact]
    public void ValidateRegisterUserRequest_NullPassword_ShouldReturnFalse()
    {
        string password = null;
        var email = "test@email.com";
        var name = _fixture.Create<string>();
        var phone = "+79781195738";
        
        var source = _fixture
            .Build<RegisterUserRequest>()
            .With(x => x.Email, email)
            .With(x => x.Name, name)
            .With(x => x.Phone, phone)
            .With(x => x.Password, password)
            .Create();
        
        _userServiceMock.Setup(x => x.IsUniqueEmailAsync(email, CancellationToken.None)).ReturnsAsync(true);
        
        var result = _validator.TestValidate(source);
        
        _userServiceMock.Verify(x => x.IsUniqueEmailAsync(email, CancellationToken.None), Times.Once);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Phone);
        result.ShouldHaveValidationErrorFor(x => x.Password).WithErrorMessage("Пароль не может быть пустым.");
    }
    
    /// <summary>
    /// Test invalid.
    /// </summary>
    [Fact]
    public void ValidateRegisterUserRequest_EmptyEmail_ShouldReturnFalse()
    {
        var password = _fixture.Create<string>();
        var email = "";
        var name = _fixture.Create<string>();
        var phone = "+79781195738";
        
        var source = _fixture
            .Build<RegisterUserRequest>()
            .With(x => x.Email, email)
            .With(x => x.Name, name)
            .With(x => x.Phone, phone)
            .With(x => x.Password, password)
            .Create();
        
        var result = _validator.TestValidate(source);
        
        _userServiceMock.Verify(x => x.IsUniqueEmailAsync(email, CancellationToken.None), Times.Never);
        result.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage("Логин не может быть пустым.");
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Phone);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }
    
    /// <summary>
    /// Test invalid.
    /// </summary>
    [Fact]
    public void ValidateRegisterUserRequest_InvalidEmail_ShouldReturnFalse()
    {
        var password = _fixture.Create<string>();
        var email = _fixture.Create<string>();
        var name = _fixture.Create<string>();
        var phone = "+79781195738";
        
        var source = _fixture
            .Build<RegisterUserRequest>()
            .With(x => x.Email, email)
            .With(x => x.Name, name)
            .With(x => x.Phone, phone)
            .With(x => x.Password, password)
            .Create();
        
        var result = _validator.TestValidate(source);
        
        result.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage("Некорректный адрес.");
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Phone);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }
    
    /// <summary>
    /// Test invalid.
    /// </summary>
    [Fact]
    public void ValidateRegisterUserRequest_EmailAlreadyExists_ShouldReturnFalse()
    {
        var password = _fixture.Create<string>();
        var email = "baobab@email.com";
        var name = _fixture.Create<string>();
        var phone = "+79781195738";
        
        var source = _fixture
            .Build<RegisterUserRequest>()
            .With(x => x.Email, email)
            .With(x => x.Name, name)
            .With(x => x.Phone, phone)
            .With(x => x.Password, password)
            .Create();
        
        _userServiceMock.Setup(x => x.IsUniqueEmailAsync(email, CancellationToken.None)).ReturnsAsync(false);
        
        var result = _validator.TestValidate(source);
        
        _userServiceMock.Verify(x => x.IsUniqueEmailAsync(email, CancellationToken.None), Times.Once);
        result.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage("Адрес уже зарегистрирован");
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Phone);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }
    
    /// <summary>
    /// Test invalid.
    /// </summary>
    [Fact]
    public void ValidateRegisterUserRequest_NullName_ShouldReturnFalse()
    {
        var password = _fixture.Create<string>();
        var email = "test@email.com";
        string name = null;
        string phone = null;
        
        var source = _fixture
            .Build<RegisterUserRequest>()
            .With(x => x.Email, email)
            .With(x => x.Name, name)
            .With(x => x.Phone, phone)
            .With(x => x.Password, password)
            .Create();
        
        _userServiceMock.Setup(x => x.IsUniqueEmailAsync(email, CancellationToken.None)).ReturnsAsync(true);
        
        var result = _validator.TestValidate(source);
        
        _userServiceMock.Verify(x => x.IsUniqueEmailAsync(email, CancellationToken.None), Times.Once);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldHaveValidationErrorFor(x => x.Name).WithErrorMessage("Имя не может быть пустым.");
        result.ShouldNotHaveValidationErrorFor(x => x.Phone);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }
    
    /// <summary>
    /// Test invalid.
    /// </summary>
    [Fact]
    public void ValidateRegisterUserRequest_InvalidPhone_ShouldReturnFalse()
    {
        var password = _fixture.Create<string>();
        var email = "test@email.com";
        var name = _fixture.Create<string>();
        var phone = _fixture.Create<string>();
        
        var source = _fixture
            .Build<RegisterUserRequest>()
            .With(x => x.Email, email)
            .With(x => x.Name, name)
            .With(x => x.Phone, phone)
            .With(x => x.Password, password)
            .Create();
        
        _userServiceMock.Setup(x => x.IsUniqueEmailAsync(email, CancellationToken.None)).ReturnsAsync(true);
        
        var result = _validator.TestValidate(source);
        
        _userServiceMock.Verify(x => x.IsUniqueEmailAsync(email, CancellationToken.None), Times.Once);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldHaveValidationErrorFor(x => x.Phone).WithErrorMessage("Некорректный телефон");
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }
}