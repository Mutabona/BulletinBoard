using AutoFixture;
using BulletinBoard.AppServices.Validators.Comments;
using BulletinBoard.Contracts.Comments;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace BulletinBoard.Tests.AppServicesTests.ValidatorsTests.Comments;

/// <summary>
/// Тесты для <see cref="AddCommentValidator"/>
/// </summary>
public class AddCommentValidatorTests
{
    private readonly AddCommentValidator _validator;
    private Fixture _fixture;

    public AddCommentValidatorTests()
    {
        _validator = new AddCommentValidator();
        _fixture = new Fixture();
    }

    /// <summary>
    /// Test valid.
    /// </summary>
    [Fact]
    public void ValidateAddComment_WithValidData_ShouldReturnTrue()
    {
        //Arrange
        var text = _fixture.Create<string>();

        var source = _fixture
            .Build<AddCommentRequest>()
            .With(x => x.Text, text)
            .Create();
        
        //Act
        var result = _validator.TestValidate(source);
        
        //Assert
        result.IsValid.Should().BeTrue();
        result.ShouldNotHaveValidationErrorFor(x => x.Text);
    }

    /// <summary>
    /// Test invalid.
    /// </summary>
    [Fact]
    public void ValidateAddComment_WithInvalidData_ShouldReturnFalse()
    {
        //Arrange
        var text = "";
        var source = new AddCommentRequest
        {
            Text = text,
        };
        
        //Act
        var result = _validator.TestValidate(source);
        
        //Assert
        result.IsValid.Should().BeFalse();
        result
            .ShouldHaveValidationErrorFor(x => x.Text)
            .WithErrorMessage("Комментарий не может быть пустым.");
    }
}