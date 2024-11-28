using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Application.Features.Auth.Commands.RevokeToken;

public record RevokeTokenCommand(string Token) : ICommand<RevokedTokenResponse>;