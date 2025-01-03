using MGH.Core.Application.Responses;

namespace Security.Application.Features.Auth.Commands.RevokeToken;

public record RevokedTokenResponse(int Id, string Token) : IResponse;
