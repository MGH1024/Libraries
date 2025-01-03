using MGH.Core.Domain.Buses.Commands;

namespace Security.Application.Features.Auth.Commands.RevokeToken;

public record RevokeTokenCommand(string Token) : ICommand<RevokedTokenResponse>;