using Application.Features.Auth.Rules;
using Application.Services.AuthService;
using Application.Services.UsersService;
using Domain;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters.GetModels;

namespace Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommand(string refreshToken, string ipAddress) : ICommand<RefreshedTokensResponse>
{
    public string RefreshToken { get; set; } = refreshToken;
    public string IpAddress { get; set; } = ipAddress;

    public RefreshTokenCommand() : this(string.Empty, string.Empty)
    {
    }
}

public class RefreshTokenCommandHandler(
    IAuthService authService,
    IUserService userService,
    IUow uow,
    AuthBusinessRules authBusinessRules)
    : ICommandHandler<RefreshTokenCommand, RefreshedTokensResponse>
{
    public async Task<RefreshedTokensResponse> Handle(RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var refreshToken =
            await authService.GetRefreshTokenByToken(request.RefreshToken, cancellationToken);
        await authBusinessRules.RefreshTokenShouldBeExists(refreshToken);

        if (refreshToken!.Revoked != null)
            await authService.RevokeDescendantRefreshTokens(
                refreshToken,
                request.IpAddress,
                reason: $"Attempted reuse of revoked ancestor token: {refreshToken.Token}",
                cancellationToken: cancellationToken
            );
        await authBusinessRules.RefreshTokenShouldBeActive(refreshToken);

        var user = await userService.GetAsync(
            new GetModel<User>
            {
                Predicate = u => u.Id == refreshToken.UserId,
                CancellationToken = cancellationToken
            });
        await authBusinessRules.UserShouldBeExistsWhenSelected(user);

        var newRefreshTkn =
            await authService.RotateRefreshToken(
                user: user!,
                refreshToken,
                request.IpAddress, cancellationToken
            );
        var addedRefreshTkn =
            await authService.AddRefreshTokenAsync(newRefreshTkn, cancellationToken);
        await authService.DeleteOldRefreshTokens(refreshToken.UserId, cancellationToken);
        await uow.CompleteAsync(cancellationToken);
        var createdAccessToken = await authService.CreateAccessTokenAsync(user!, cancellationToken);

        var refreshedTokensResponse = new RefreshedTokensResponse
            { AccessToken = createdAccessToken, RefreshTkn = addedRefreshTkn };
        return refreshedTokensResponse;
    }
}