using MGH.Core.Domain.Buses.Commands;
using Security.Application.Features.Auth.Rules;
using Security.Application.Features.Auth.Services;
using Security.Domain;

namespace Security.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler(IAuthService authService, IUow uow, IAuthBusinessRules authBusinessRules)
    : ICommandHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await uow.RefreshToken.GetByTokenAsync(request.RefreshToken, cancellationToken);
        await authBusinessRules.RefreshTokenShouldBeExists(refreshToken);

        if (refreshToken!.Revoked != null)
        {
            var reason = $"Attempted reuse of revoked ancestor token:" + $" {refreshToken.Token}";
            await authService.RevokeDescendantRefreshTokens(refreshToken, reason, cancellationToken);
        }

        await authBusinessRules.RefreshTokenShouldBeActive(refreshToken);

        var user = await uow.User.GetAsync(refreshToken.UserId, cancellationToken);
        await authBusinessRules.UserShouldBeExistsWhenSelected(user);

        var newRefreshToken = await authService.RotateRefreshToken(user!, refreshToken, cancellationToken);
        var addedRefreshTkn = await authService.AddRefreshTokenAsync(newRefreshToken, cancellationToken);
        await authService.DeleteOldRefreshTokens(refreshToken.UserId, cancellationToken);
        await uow.CompleteAsync(cancellationToken);
        var createdAccessToken = await authService.CreateAccessTokenAsync(user!, cancellationToken);

        return new RefreshTokenResponse(createdAccessToken.Token,createdAccessToken.Expiration,
            addedRefreshTkn.Token, addedRefreshTkn.Expires);
    }
}