using AutoFixture;
using BulletinBoard.AppServices.Contexts.Users.Services;
using BulletinBoard.AppServices.Validators.Users;
using BulletinBoard.Contracts.Users;
using FluentValidation.TestHelper;
using Moq;

namespace BulletinBoard.Tests.AppServicesTests.ValidatorsTests.Users;

/// <summary>
/// Тесты для <see cref="LoginUserRequestValidator"/>
/// </summary>
public class LoginUserRequestValidatorTests
{
    private readonly LoginUserRequestValidator _validator;
    private readonly Fixture _fixture;

    public LoginUserRequestValidatorTests()
    {
        _fixture = new Fixture();
        _validator = new LoginUserRequestValidator();
    }

    /// <summary>
    /// Test valid.
    /// </summary>
    [Fact]
    public void ValidateLoginUserRequest_WithValidData_ShouldReturnTrue()
    {
        var email = "barabaz@gmail.com";
        var password = _fixture.Create<string>();
        
        var source = _fixture
            .Build<LoginUserRequest>()
            .With(x => x.Email, email)
            .With(x => x.Password, password)
            .Create();
        
        var result = _validator.TestValidate(source);
        
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }

    /// <summary>
    /// Test invalid.
    /// </summary>
    [Fact]
    public void ValidateLoginUserRequest_WithInvalidEmail_ShouldReturnFalse()
    {
        var email = _fixture.Create<string>();
        var password = _fixture.Create<string>();
        
        var source = _fixture
            .Build<LoginUserRequest>()
            .With(x => x.Email, email)
            .With(x => x.Password, password)
            .Create();
        
        var result = _validator.TestValidate(source);

        result.ShouldNotHaveValidationErrorFor(x => x.Password);
        result.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage("Некорректный адрес.");
    }
    
    /// <summary>
    /// Test invalid.
    /// </summary>
    [Fact]
    public void ValidateLoginUserRequest_WithNullEmail_ShouldReturnFalse()
    {
        string email = null;
        var password = _fixture.Create<string>();
        
        var source = _fixture
            .Build<LoginUserRequest>()
            .With(x => x.Email, email)
            .With(x => x.Password, password)
            .Create();
        
        var result = _validator.TestValidate(source);
        
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
        result.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage("Логин не может быть пустым.");
    }
    
    /// <summary>
    /// Test invalid.
    /// </summary>
    [Fact]
    public void ValidateLoginUserRequest_WithEmptyPassword_ShouldReturnFalse()
    {
        var email = "barabaz@gmail.com";
        string password = null;
        
        var source = _fixture
            .Build<LoginUserRequest>()
            .With(x => x.Email, email)
            .With(x => x.Password, password)
            .Create();
        
        var result = _validator.TestValidate(source);

        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldHaveValidationErrorFor(x => x.Password).WithErrorMessage("Пароль не может быть пустым.");
    }
    
    /// <summary>
    /// Test invalid.
    /// </summary>
    [Fact]
    public void ValidateLoginUserRequest_WithInvalidPassword_ShouldReturnFalse()
    {
        var email = "barabaz@gmail.com";
        string password = "1111";
        
        var source = _fixture
            .Build<LoginUserRequest>()
            .With(x => x.Email, email)
            .With(x => x.Password, password)
            .Create();
        
        var result = _validator.TestValidate(source);

        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}