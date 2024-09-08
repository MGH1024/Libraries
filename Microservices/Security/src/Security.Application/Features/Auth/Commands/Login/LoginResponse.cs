using MGH.Core.Application.Responses;
using MGH.Core.Infrastructure.Securities.Security.Enums;
using MGH.Core.Infrastructure.Securities.Security.JWT;

namespace Application.Features.Auth.Commands.Login;

public class LoginResponse : IResponse
{
    public AccessToken AccessToken { get; set; }
    public MGH.Core.Infrastructure.Securities.Security.Entities.RefreshTkn RefreshTkn { get; set; }
    public AuthenticatorType RequiredAuthenticatorType { get; set; }

    public LoginHttpResponse ToHttpResponse() =>
        new() { AccessToken = AccessToken, RequiredAuthenticatorType = RequiredAuthenticatorType };
}
