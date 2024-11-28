using Domain;
using MGH.Core.Domain.Buses.Commands;
using Application.Features.Auth.Rules;
using Application.Features.Auth.Services;
using MGH.Core.Infrastructure.Persistence.EF.Models.Filters.GetModels;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler(IAuthService authService, IUow uow, IAuthBusinessRules authBusinessRules)
    : ICommandHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await uow.RefreshToken
            .GetAsync(new GetModel<RefreshTkn> { Predicate = r => r.Token == request.RefreshToken });

        await authBusinessRules.RefreshTokenShouldBeExists(refreshToken);

        if (refreshToken!.Revoked != null)
        {
            var reason = $"Attempted reuse of revoked ancestor token:" + $" {refreshToken.Token}";
            await authService.RevokeDescendantRefreshTokens(refreshToken, reason, cancellationToken);
        }

        await authBusinessRules.RefreshTokenShouldBeActive(refreshToken);

        var user = await uow.User.GetAsync(refreshToken.UserId, cancellationToken);
        await authBusinessRules.UserShouldBeExistsWhenSelected(user);

        var newRefreshTkn = await authService.RotateRefreshToken(user!, refreshToken, cancellationToken);
        var addedRefreshTkn = await authService.AddRefreshTokenAsync(newRefreshTkn, cancellationToken);
        await authService.DeleteOldRefreshTokens(refreshToken.UserId, cancellationToken);
        await uow.CompleteAsync(cancellationToken);
        var createdAccessToken = await authService.CreateAccessTokenAsync(user!, cancellationToken);

        return new RefreshTokenResponse(createdAccessToken, addedRefreshTkn);
    }
}