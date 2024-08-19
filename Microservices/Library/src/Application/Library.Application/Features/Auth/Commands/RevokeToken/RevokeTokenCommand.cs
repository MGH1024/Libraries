using Application.Features.Auth.Rules;
using Application.Services.AuthService;
using AutoMapper;
using MGH.Core.Domain.Buses.Commands;

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
        MGH.Core.Infrastructure.Securities.Security.Entities.RefreshTkn refreshTkn =
            await authService.GetRefreshTokenByToken(request.Token, cancellationToken);
        await authBusinessRules.RefreshTokenShouldBeExists(refreshTkn);
        await authBusinessRules.RefreshTokenShouldBeActive(refreshTkn!);

        await authService.RevokeRefreshToken(tkn: refreshTkn!, request.IpAddress, reason: "Revoked without replacement",
            cancellationToken: cancellationToken);

        RevokedTokenResponse revokedTokenResponse = mapper.Map<RevokedTokenResponse>(refreshTkn);
        return revokedTokenResponse;
    }
}