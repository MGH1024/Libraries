using MGH.Core.Application.Responses;
using MGH.Core.Infrastructure.Securities.Security.JWT;

namespace Application.Features.Auth.Commands.Register;

public class RegisteredResponse(
    AccessToken accessToken,
    MGH.Core.Infrastructure.Securities.Security.Entities.RefreshTkn refreshTkn)
    : IResponse
{
    public AccessToken AccessToken { get; set; } = accessToken;
    public MGH.Core.Infrastructure.Securities.Security.Entities.RefreshTkn RefreshTkn { get; set; } = refreshTkn;

    public RegisteredResponse() : this(null!, null!)
    {
    }
}
