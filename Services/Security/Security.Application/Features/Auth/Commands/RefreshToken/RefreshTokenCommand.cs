using MGH.Core.Domain.Buses.Commands;

namespace Security.Application.Features.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : ICommand<RefreshTokenResponse>;