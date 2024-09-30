using MGH.Core.Infrastructure.Securities.Security.JWT;

namespace Application.Features.Auth.Commands.Login;

public class LoginHttpResponse
{
    public bool IsSuccess { get; set; }
    public AccessToken AccessToken { get; set; }
}