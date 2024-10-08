using Domain;
using MGH.Core.Domain.Buses.Commands;
using Application.Features.Auth.Rules;
using Application.Services.AuthService;

namespace Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommand(string refreshToken, string ipAddress) : ICommand<RefreshTokenResponse>
{
    public string RefreshToken { get; set; } = refreshToken;
    public string IpAddress { get; set; } = ipAddress;

    public RefreshTokenCommand() : this(string.Empty, string.Empty)
    {
    }
}

public class RefreshTokenCommandHandler(IAuthService authService, IUow uow, AuthBusinessRules authBusinessRules) 
    : ICommandHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await authService.GetRefreshTokenByToken(request.RefreshToken, cancellationToken);
        await authBusinessRules.RefreshTokenShouldBeExists(refreshToken);

        if (refreshToken!.Revoked != null)
        {
            var reason = $"Attempted reuse of revoked ancestor token:" + $" {refreshToken.Token}";
            await authService.RevokeDescendantRefreshTokens(refreshToken, request.IpAddress, reason, cancellationToken);
        }
        await authBusinessRules.RefreshTokenShouldBeActive(refreshToken);
        
        var user = await uow.User.GetAsync(refreshToken.UserId, cancellationToken);
        await authBusinessRules.UserShouldBeExistsWhenSelected(user);

        var newRefreshTkn = await authService.RotateRefreshToken(user!, refreshToken, request.IpAddress, cancellationToken);
        var addedRefreshTkn = await authService.AddRefreshTokenAsync(newRefreshTkn, cancellationToken);
        await authService.DeleteOldRefreshTokens(refreshToken.UserId, cancellationToken);
        await uow.CompleteAsync(cancellationToken);
        var createdAccessToken = await authService.CreateAccessTokenAsync(user!, cancellationToken);

        return new RefreshTokenResponse { AccessToken = createdAccessToken, RefreshTkn = addedRefreshTkn };
    }
}