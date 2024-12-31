using MGH.Core.Application.Responses;

namespace Application.Features.Auth.Commands.RevokeToken;

public record RevokedTokenResponse(int Id, string Token) : IResponse;
