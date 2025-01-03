using FluentValidation.TestHelper;
using Security.Application.Features.Auth.Commands.UserLogin;

namespace Security.Test.Application.Features.Auth.Commands.UserLogin;

public class UserLoginCommandDtoTest
{
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void GivenEmptyEmail_WhenValidate_ThenWillInvalid(string? email)
    {
        var userLoginDto = new UserLoginDtoBuilder()
            .WithEmail(email)
            .Build();

        var validator = new UserLoginCommandDtoValidator();
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

        var validator = new UserLoginCommandDtoValidator();
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

        var validator = new UserLoginCommandDtoValidator();
        var result = validator.TestValidate(userLoginDto);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}