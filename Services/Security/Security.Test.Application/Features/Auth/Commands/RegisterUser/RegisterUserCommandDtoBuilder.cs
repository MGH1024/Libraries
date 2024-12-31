namespace Security.Test.Features.Auth.Commands.RegisterUser;

public class RegisterUserCommandDtoBuilder
{
    private string? _email;
    private string? _password;
    private string? _firstName;
    private string? _lastName;

    public RegisterUserCommandDtoBuilder WithEmail(string? email)
    {
        _email = email;
        return this;
    }

    public RegisterUserCommandDtoBuilder WithPassword(string? password)
    {
        _password = password;
        return this;
    }

    public RegisterUserCommandDtoBuilder WithFirstName(string? firstName)
    {
        _firstName = firstName;
        return this;
    }

    public RegisterUserCommandDtoBuilder WithLastName(string? lastname)
    {
        _lastName = lastname;
        return this;
    }

    public Application.Features.Auth.Commands.RegisterUser.RegisterUserCommandDto Build()
    {
        return new Application.Features.Auth.Commands.RegisterUser.RegisterUserCommandDto(_email, _password, _firstName, _lastName);
    }
}