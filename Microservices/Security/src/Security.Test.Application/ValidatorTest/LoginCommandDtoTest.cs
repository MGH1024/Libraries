using FluentValidation.TestHelper;
using Security.Test.Builders.Validators;
using Application.Features.Auth.Commands.Login;
using Security.Test.Builders.Validators.Auth;

namespace Security.Test.ValidatorTest;

public class LoginCommandDtoTest
{
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void GivenEmptyEmail_WhenValidate_ThenWillInvalid(string? email)
    {
        var userLoginDto = new UserLoginDtoBuilder()
            .WithEmail(email)
            .Build();

        var validator = new LoginCommandDtoValidator();
        var result = validator.TestValidate(userLoginDto);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void GivenEmptyPassword_WhenValidate_ThenWillInvalid(string? password)
    {
        var userLoginDto = new UserLoginDtoBuilder()
            .WithPassword(password)
            .Build();

        var validator = new LoginCommandDtoValidator();
        var result = validator.TestValidate(userLoginDto);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
    
    
    [Theory]
    [InlineData("p")]
    [InlineData("pa")]
    [InlineData("pas")]
    public void GivenLessThanFourCharacterPassword_WhenValidate_ThenWillInvalid(string? password)
    {
        var userLoginDto = new UserLoginDtoBuilder()
            .WithPassword(password)
            .Build();

        var validator = new LoginCommandDtoValidator();
        var result = validator.TestValidate(userLoginDto);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}