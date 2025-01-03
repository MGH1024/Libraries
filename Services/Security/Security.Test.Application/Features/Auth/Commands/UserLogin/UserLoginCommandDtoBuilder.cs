using Security.Application.Features.Auth.Commands.UserLogin;

namespace Security.Test.Application.Features.Auth.Commands.UserLogin;

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

    public UserLoginCommandDto Build()
    {
        return new UserLoginCommandDto(_email, _password);
    }
}