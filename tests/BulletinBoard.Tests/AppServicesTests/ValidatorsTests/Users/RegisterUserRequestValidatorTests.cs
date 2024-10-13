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
    private readonly Fixture _fixture;

    public RegisterUserRequestValidatorTests()
    {
        _validator = new RegisterUserRequestValidator();
        _fixture = new Fixture();
    }

    /// <summary>
    /// Test valid.
    /// </summary>
    [Fact]
    public void ValidateRegisterUserRequest_ValidData_ShouldReturnTrue()
    {
        //Arrange
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
        
        //Act
        var result = _validator.TestValidate(source);
        
        //Assert
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
        //Arrange
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
        
        //Act
        var result = _validator.TestValidate(source);
        
        //Assert
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
        //Arrange
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
        
        //Act
        var result = _validator.TestValidate(source);
        
        //Assert
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
        //Arrange
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
        
        //Act
        var result = _validator.TestValidate(source);
        
        //Assert
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
        //Arrange
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
        
        //Act
        var result = _validator.TestValidate(source);
        
        //Assert
        result.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage("Некорректный адрес.");
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
        //Arrange
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
        
        //Act
        var result = _validator.TestValidate(source);
        
        //Assert
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
        //Arrange
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
        
        //Act
        var result = _validator.TestValidate(source);
        
        //Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldHaveValidationErrorFor(x => x.Phone).WithErrorMessage("Некорректный телефон");
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }
}