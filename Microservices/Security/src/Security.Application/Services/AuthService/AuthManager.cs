using Domain;
using Microsoft.Extensions.Options;
using MGH.Core.Infrastructure.Public;
using MGH.Core.Persistence.Models.Filters.GetModels;
using MGH.Core.Infrastructure.Securities.Security.JWT;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Application.Services.AuthService;

public class AuthManager(
    IUow uow,
    ITokenHelper tokenHelper,
    IDateTime time,
    IOptions<TokenOptions> options) : IAuthService
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
        await uow.RefreshToken.DeleteRangeAsync(refreshTokens,false);
    }

    public async Task<RefreshTkn> GetRefreshTokenByToken(string token, CancellationToken cancellationToken)
    {
        var refreshToken = await uow.RefreshToken.GetAsync(new GetModel<RefreshTkn> { Predicate = r => r.Token == token });
        return refreshToken;
    }

    public async Task RevokeRefreshTokenAsync(
        RefreshTkn refreshToken,
        string ipAddress,
        string reason = null,
        string replacedByToken = null,
        CancellationToken cancellationToken = default)
    {
        refreshToken.Revoked = time.IranNow;
        refreshToken.RevokedByIp = ipAddress;
        refreshToken.ReasonRevoked = reason;
        refreshToken.ReplacedByToken = replacedByToken;
        await uow.RefreshToken.UpdateAsync(refreshToken, cancellationToken);
    }


    public async Task<RefreshTkn> RotateRefreshToken(User user, RefreshTkn refreshTkn, string ipAddress,
        CancellationToken cancellationToken)
    {
        var newRefreshTkn = tokenHelper.CreateRefreshToken(user, ipAddress);
        await RevokeRefreshTokenAsync(refreshTkn, ipAddress, reason: "Replaced by new token",
            newRefreshTkn.Token, cancellationToken);
        return newRefreshTkn;
    }

    public async Task RevokeDescendantRefreshTokens(RefreshTkn refreshTkn, string ipAddress, string reason,
        CancellationToken cancellationToken)
    {
        var childToken = await uow.RefreshToken.GetAsync(new GetModel<RefreshTkn>
        {
            Predicate = r => r.Token == refreshTkn.ReplacedByToken
        });

        if (childToken?.Revoked != null && childToken.Expires <= DateTime.UtcNow)
            await RevokeRefreshTokenAsync(childToken, ipAddress, reason, childToken.Token, cancellationToken);
        else
            await RevokeDescendantRefreshTokens(refreshTkn: childToken!, ipAddress, reason, cancellationToken);
    }

    public Task<RefreshTkn> CreateRefreshToken(User user, string ipAddress, CancellationToken cancellationToken)
    {
        var refreshToken = tokenHelper.CreateRefreshToken(user, ipAddress);
        return Task.FromResult(refreshToken);
    }
}