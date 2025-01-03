using MGH.Core.Infrastructure.Persistence.Models.Filters.GetModels;
using MGH.Core.Infrastructure.Public;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Infrastructure.Securities.Security.Hashing;
using MGH.Core.Infrastructure.Securities.Security.JWT;
using Microsoft.Extensions.Options;
using Security.Application.Features.Users.Extensions;
using Security.Domain;

namespace Security.Application.Features.Auth.Services;

public class AuthManager(IUow uow, ITokenHelper tokenHelper, IDateTime time, IOptions<TokenOptions> options)
    : IAuthService
{
    private readonly TokenOptions _tokenOptions = options.Value;

    public async Task<AccessToken> CreateAccessTokenAsync(User user, CancellationToken cancellationToken)
    {
        var operationClaims = await uow.UserOperationClaim.GetOperationClaim(user, cancellationToken);
        return tokenHelper.CreateToken(user, operationClaims);
    }

    public async Task<RefreshToken> AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        return await uow.RefreshToken.AddAsync(refreshToken, cancellationToken);
    }

    public async Task DeleteOldRefreshTokens(int userId, CancellationToken cancellationToken)
    {
        var refreshTokens =
            await uow.RefreshToken.GetRefreshTokenByUserId(userId, _tokenOptions.RefreshTokenTtl, cancellationToken);
        await uow.RefreshToken.DeleteRangeAsync(refreshTokens);
    }

    public async Task<RefreshToken> GetRefreshTokenByToken(string token, CancellationToken cancellationToken)
    {
        var refreshToken =
            await uow.RefreshToken.GetAsync(new GetModel<RefreshToken> { Predicate = r => r.Token == token });
        return refreshToken;
    }

    public async Task<RefreshToken> RotateRefreshToken(User user, RefreshToken refreshToken,
        CancellationToken cancellationToken)
    {
        var newRefreshTkn = tokenHelper.CreateRefreshToken(user);
        await RevokeRefreshTokenAsync(refreshToken, reason: "Replaced by new token",
            newRefreshTkn.Token, cancellationToken);
        return newRefreshTkn;
    }

    public async Task RevokeDescendantRefreshTokens(RefreshToken refreshToken, string reason,
        CancellationToken cancellationToken)
    {
        var childToken = await uow.RefreshToken.GetAsync(new GetModel<RefreshToken>
        {
            Predicate = r => r.Token == refreshToken.ReplacedByToken
        });

        if (childToken?.Revoked != null && childToken.Expires <= DateTime.UtcNow)
            await RevokeRefreshTokenAsync(childToken, reason, childToken.Token, cancellationToken);
        else
            await RevokeDescendantRefreshTokens(refreshToken: childToken!, reason, cancellationToken);
    }

    public void SetHashPassword(string password, User user)
    {
        var hashingHelperModel = HashingHelper.CreatePasswordHash(password);
        user.SetHashPassword(hashingHelperModel);
    }
    public Task<RefreshToken> CreateRefreshToken(User user)
    {
        var refreshToken = tokenHelper.CreateRefreshToken(user);
        return Task.FromResult(refreshToken);
    }

    private async Task RevokeRefreshTokenAsync(
        RefreshToken refreshToken,
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