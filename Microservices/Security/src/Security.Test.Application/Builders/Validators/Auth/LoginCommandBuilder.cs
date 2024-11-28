using Application.Features.Auth.Commands.Login;

namespace Security.Test.Builders.Validators.Auth;

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

    public LoginCommandDto Build()
    {
        return new LoginCommandDto(_email, _password);
    }
}