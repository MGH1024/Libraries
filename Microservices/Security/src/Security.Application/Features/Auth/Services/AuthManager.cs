using Domain;
using MGH.Core.Infrastructure.Persistence.EF.Models.Filters.GetModels;
using MGH.Core.Infrastructure.Public;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Infrastructure.Securities.Security.JWT;
using Microsoft.Extensions.Options;

namespace Application.Features.Auth.Services;

public class AuthManager(IUow uow, ITokenHelper tokenHelper, IDateTime time, IOptions<TokenOptions> options) : IAuthService
{
    private readonly TokenOptions _tokenOptions = options.Value;

    public async Task<AccessToken> CreateAccessTokenAsync(User user, CancellationToken cancellationToken)
    {
        var operationClaims = await uow.UserOperationClaim.GetOperationClaim(user, cancellationToken);
        return tokenHelper.CreateToken(user, operationClaims);
    }

    public async Task<RefreshTkn> AddRefreshTokenAsync(RefreshTkn refreshTkn, CancellationToken cancellationToken)
    {
        return await uow.RefreshToken.AddAsync(refreshTkn, cancellationToken);
    }

    public async Task DeleteOldRefreshTokens(int userId, CancellationToken cancellationToken)
    {
        var refreshTokens = await uow.RefreshToken.GetRefreshTokenByUserId(userId, _tokenOptions.RefreshTokenTtl, cancellationToken);
        await uow.RefreshToken.DeleteRangeAsync(refreshTokens);
    }

    public async Task<RefreshTkn> GetRefreshTokenByToken(string token, CancellationToken cancellationToken)
    {
        var refreshToken = await uow.RefreshToken.GetAsync(new GetModel<RefreshTkn> { Predicate = r => r.Token == token });
        return refreshToken;
    }
    public async Task<RefreshTkn> RotateRefreshToken(User user, RefreshTkn refreshTkn,
        CancellationToken cancellationToken)
    {
        var newRefreshTkn = tokenHelper.CreateRefreshToken(user);
        await RevokeRefreshTokenAsync(refreshTkn, reason: "Replaced by new token",
            newRefreshTkn.Token, cancellationToken);
        return newRefreshTkn;
    }

    public async Task RevokeDescendantRefreshTokens(RefreshTkn refreshTkn, string reason,
        CancellationToken cancellationToken)
    {
        var childToken = await uow.RefreshToken.GetAsync(new GetModel<RefreshTkn>
        {
            Predicate = r => r.Token == refreshTkn.ReplacedByToken
        });

        if (childToken?.Revoked != null && childToken.Expires <= DateTime.UtcNow)
            await RevokeRefreshTokenAsync(childToken, reason, childToken.Token, cancellationToken);
        else
            await RevokeDescendantRefreshTokens(refreshTkn: childToken!, reason, cancellationToken);
    }

    public Task<RefreshTkn> CreateRefreshToken(User user, CancellationToken cancellationToken)
    {
        var refreshToken = tokenHelper.CreateRefreshToken(user);
        return Task.FromResult(refreshToken);
    }
    
    private async Task RevokeRefreshTokenAsync(
        RefreshTkn refreshToken,
        string reason = null,
        string replacedByToken = null,
        CancellationToken cancellationToken = default)
    {
        refreshToken.Revoked = time.IranNow;
        refreshToken.ReasonRevoked = reason;
        refreshToken.ReplacedByToken = replacedByToken;
        await uow.RefreshToken.UpdateAsync(refreshToken, cancellationToken);
    }
}