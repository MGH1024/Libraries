using MGH.Core.Application.Responses;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Infrastructure.Securities.Security.JWT;

namespace Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenResponse(AccessToken accessToken, RefreshTkn refreshTkn) : IResponse
{
    public AccessToken AccessToken { get; set; } = accessToken;
    public  RefreshTkn RefreshTkn { get; set; } = refreshTkn;

    public RefreshTokenResponse() : this(null!, null!)
    {
    }
}
