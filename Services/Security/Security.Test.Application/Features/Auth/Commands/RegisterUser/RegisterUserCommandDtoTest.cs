using FluentValidation.TestHelper;
using Security.Application.Features.Auth.Commands.RegisterUser;

namespace Security.Test.Application.Features.Auth.Commands.RegisterUser;

public class RegisterUserCommandDtoTest
{
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void GivenEmptyEmail_WhenValidate_ThenWillInvalid(string? email)
    {
        var registerUserCommandDto = new RegisterUserCommandDtoBuilder()
            .WithEmail(email)
            .Build();

        var validator = new RegisterUserCommandDtoValidator();
        var result = validator.TestValidate(registerUserCommandDto);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
    
    [Theory]
    [InlineData("m.com")]
    public void GivenNonTrueEmail_WhenValidate_ThenWillInvalid(string? email)
    {
        var registerUserCommandDto = new RegisterUserCommandDtoBuilder()
            .WithEmail(email)
            .Build();

        var validator = new RegisterUserCommandDtoValidator();
        var result = validator.TestValidate(registerUserCommandDto);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void GivenEmptyFirstName_WhenValidate_ThenWillInvalid(string? firstName)
    {
        var registerUserCommandDto = new RegisterUserCommandDtoBuilder()
            .WithFirstName(firstName)
            .Build();

        var validator = new RegisterUserCommandDtoValidator();
        var result = validator.TestValidate(registerUserCommandDto);
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }
    
    [Theory]
    [InlineData("d")]
    public void GivenFirstNameWithLowerThanTwoCharacter_WhenValidate_ThenWillInvalid(string? firstName)
    {
        var registerUserCommandDto = new RegisterUserCommandDtoBuilder()
            .WithFirstName(firstName)
            .Build();

        var validator = new RegisterUserCommandDtoValidator();
        var result = validator.TestValidate(registerUserCommandDto);
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void GivenEmptyLastName_WhenValidate_ThenWillInvalid(string? lastName)
    {
        var registerUserCommandDto = new RegisterUserCommandDtoBuilder()
            .WithLastName(lastName)
            .Build();

        var validator = new RegisterUserCommandDtoValidator();
        var result = validator.TestValidate(registerUserCommandDto);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }
    
    [Theory]
    [InlineData("d")]
    public void GivenLastNameWithLowerThanTwoCharacter_WhenValidate_ThenWillInvalid(string? lastName)
    {
        var registerUserCommandDto = new RegisterUserCommandDtoBuilder()
            .WithLastName(lastName)
            .Build();

        var validator = new RegisterUserCommandDtoValidator();
        var result = validator.TestValidate(registerUserCommandDto);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void GivenEmptyPassword_WhenValidate_ThenWillInvalid(string? password)
    {
        var registerUserCommandDto = new RegisterUserCommandDtoBuilder()
            .WithPassword(password)
            .Build();

        var validator = new RegisterUserCommandDtoValidator();
        var result = validator.TestValidate(registerUserCommandDto);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
    
    [Theory]
    [InlineData("dww")]
    public void GivenLastNameWithLowerThanFourCharacter_WhenValidate_ThenWillInvalid(string? password)
    {
        var registerUserCommandDto = new RegisterUserCommandDtoBuilder()
            .WithPassword(password)
            .Build();

        var validator = new RegisterUserCommandDtoValidator();
        var result = validator.TestValidate(registerUserCommandDto);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}