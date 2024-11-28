using MGH.Core.Domain.Buses.Commands;

namespace Application.Features.Auth.Commands.RevokeToken;

public record RevokeTokenCommand(string Token) : ICommand<RevokedTokenResponse>;