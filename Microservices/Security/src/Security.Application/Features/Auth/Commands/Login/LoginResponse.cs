using MGH.Core.Application.Responses;
using MGH.Core.Infrastructure.Securities.Security.Enums;
using MGH.Core.Infrastructure.Securities.Security.JWT;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Application.Features.Auth.Commands.Login;

public class LoginResponse : IResponse
{
    public AccessToken AccessToken { get; set; }
    public RefreshTkn RefreshTkn { get; set; }
    public bool IsSuccess { get; set; }
    public AuthenticatorType RequiredAuthenticatorType { get; set; }

    public LoginHttpResponse ToHttpResponse() => new()
    {
        AccessToken = AccessToken,
        RequiredAuthenticatorType = RequiredAuthenticatorType,
        IsSuccess = IsSuccess
    };
}
