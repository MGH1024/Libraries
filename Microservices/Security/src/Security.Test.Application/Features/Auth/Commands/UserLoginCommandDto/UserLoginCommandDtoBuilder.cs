namespace Security.Test.Features.Auth.Commands.UserLoginCommandDto;

public class UserLoginDtoBuilder
{
    private string? _email;
    private string? _password;

    public UserLoginDtoBuilder WithEmail(string? email)
    {
        _email = email;
        return this;
    }

    public UserLoginDtoBuilder WithPassword(string? password)
    {
        _password = password;
        return this;
    }

    public Application.Features.Auth.Commands.UserLogin.UserLoginCommandDto Build()
    {
        return new Application.Features.Auth.Commands.UserLogin.UserLoginCommandDto(_email, _password);
    }
}