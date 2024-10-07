using MGH.Core.Application.Responses;
using MGH.Core.Infrastructure.Securities.Security.JWT;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Application.Features.Auth.Commands.Register;

public class RegisteredResponse(
    AccessToken accessToken, RefreshTkn refreshTkn)
    : IResponse
{
    public AccessToken AccessToken { get; set; } = accessToken;
    public RefreshTkn RefreshTkn { get; set; } = refreshTkn;

    public RegisteredResponse() : this(null!, null!)
    {
    }
}
