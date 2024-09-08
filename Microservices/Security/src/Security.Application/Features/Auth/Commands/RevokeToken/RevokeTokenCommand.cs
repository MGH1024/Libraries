using Application.Features.Auth.Rules;
using Application.Services.AuthService;
using AutoMapper;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Application.Features.Auth.Commands.RevokeToken;

public class RevokeTokenCommand(string token, string ipAddress) : ICommand<RevokedTokenResponse>
{
    public string Token { get; set; } = token;
    public string IpAddress { get; set; } = ipAddress;

    public RevokeTokenCommand() : this(string.Empty, string.Empty)
    {
    }
}

public class RevokeTokenCommandHandler(
    IAuthService authService,
    AuthBusinessRules authBusinessRules,
    IMapper mapper)
    : ICommandHandler<RevokeTokenCommand, RevokedTokenResponse>
{
    public async Task<RevokedTokenResponse> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshTkn = await authService.GetRefreshTokenByToken(request.Token, cancellationToken);
        await authBusinessRules.RefreshTokenShouldBeExists(refreshTkn);
        await authBusinessRules.RefreshTokenShouldBeActive(refreshTkn!);

        await authService.RevokeRefreshTokenAsync( refreshTkn!, request.IpAddress, reason: "Revoked without replacement",
            cancellationToken: cancellationToken);

        var revokedTokenResponse = mapper.Map<RevokedTokenResponse>(refreshTkn);
        return revokedTokenResponse;
    }
}