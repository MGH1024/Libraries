using MGH.Core.Application.Responses;

namespace Application.Features.Auth.Commands.RevokeToken;

public class RevokedTokenResponse(int id, string token) : IResponse
{
    public int Id { get; set; } = id;
    public string Token { get; set; } = token;

    public RevokedTokenResponse() : this(0, string.Empty)
    {
    }
}
