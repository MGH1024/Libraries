using MGH.Core.Infrastructure.Securities.Security.Enums;
using MGH.Core.Infrastructure.Securities.Security.JWT;

namespace Application.Features.Auth.Commands.Login;

public class LoginHttpResponse
{
    public AccessToken AccessToken { get; set; }
    public AuthenticatorType? RequiredAuthenticatorType { get; set; }
}