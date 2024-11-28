using Application.Features.Auth.Rules;
using Application.Services.AuthService;
using AutoMapper;
using MGH.Core.Domain.Buses.Commands;

namespace Application.Features.Auth.Commands.RevokeToken;

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
        await authService.RevokeRefreshTokenAsync(refreshTkn!, "Revoked without replacement", null, cancellationToken);
        return mapper.Map<RevokedTokenResponse>(refreshTkn);
    }
}