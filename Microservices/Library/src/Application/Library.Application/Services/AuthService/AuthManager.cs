using Domain;
using Application.Features.Auth.Rules;
using Microsoft.Extensions.Configuration;
using MGH.Core.Persistence.Models.Filters.GetModels;
using MGH.Core.Infrastructure.Securities.Security.JWT;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Application.Services.AuthService;

public class AuthManager : IAuthService
{
    private readonly IUow _uow;
    private readonly ITokenHelper _tokenHelper;
    private readonly TokenOptions _tokenOptions;
    private readonly AuthBusinessRules _authBusinessRules;

    public AuthManager(
        IUow uow,
        ITokenHelper tokenHelper,
        IConfiguration configuration,
        AuthBusinessRules authBusinessRules
    )
    {
        _uow = uow;
        _tokenHelper = tokenHelper;
        _authBusinessRules = authBusinessRules;

        const string tokenOptionsConfigurationSection = "TokenOptions";
        _tokenOptions =
            configuration.GetSection(tokenOptionsConfigurationSection).Get<TokenOptions>()
            ?? throw new NullReferenceException(
                $"\"{tokenOptionsConfigurationSection}\" section cannot found in configuration");
    }

    public async Task<AccessToken> CreateAccessToken(User user, CancellationToken cancellationToken)
    {
        var operationClaims = await _uow.UserOperationClaim.GetOperationClaim(user, cancellationToken);
        var accessToken = _tokenHelper.CreateToken(user, operationClaims);
        return accessToken;
    }
    public async Task<RefreshTkn> AddRefreshToken(RefreshTkn refreshTkn, CancellationToken cancellationToken)
    {
        var addedRefreshToken = await _uow.RefreshToken.AddAsync(refreshTkn, cancellationToken);
        return addedRefreshToken;
    }

    public async Task DeleteOldRefreshTokens(int userId, CancellationToken cancellationToken)
    {
        var refreshTokens = await _uow.RefreshToken.GetRefreshTokenByUserId(userId,
            _tokenOptions.RefreshTokenTTL, cancellationToken);

        _uow.RefreshToken.DeleteRange(refreshTokens);
    }

    public Task RevokeDescendantRefreshTokens(RefreshTkn refreshTkn, string ipAddress, CancellationToken cancellationToken,
        string reason)
    {
        throw new NotImplementedException();
    }

    public async Task<RefreshTkn> GetRefreshTokenByToken(string token, CancellationToken cancellationToken)
    {
        var refreshToken = await _uow.RefreshToken.GetAsync(new GetModel<RefreshTkn>
            { Predicate = r => r.Token == token });
        return refreshToken;
    }

    public async Task RevokeRefreshToken(RefreshTkn refreshTkn, string ipAddress, string reason = null,
        CancellationToken cancellationToken = default, string replacedByToken = null
    )
    {
        refreshTkn.Revoked = DateTime.UtcNow;
        refreshTkn.RevokedByIp = ipAddress;
        refreshTkn.ReasonRevoked = reason;
        refreshTkn.ReplacedByToken = replacedByToken;
        _uow.RefreshToken.Update(refreshTkn, cancellationToken);
    }

    public Task RevokeRefreshToken(RefreshTkn tkn, string ipAddress, CancellationToken cancellationToken, string reason = null,
        string replacedByToken = null)
    {
        throw new NotImplementedException();
    }

    public async Task<RefreshTkn> RotateRefreshToken(User user, RefreshTkn refreshTkn, string ipAddress,
        CancellationToken cancellationToken)
    {
        RefreshTkn newRefreshTkn = _tokenHelper.CreateRefreshToken(user, ipAddress);
        await RevokeRefreshToken(refreshTkn, ipAddress, reason: "Replaced by new token", cancellationToken,
            newRefreshTkn.Token);
        return newRefreshTkn;
    }

    public async Task RevokeDescendantRefreshTokens(RefreshTkn refreshTkn, string ipAddress, string reason,
        CancellationToken cancellationToken)
    {
        var childToken = await _uow.RefreshToken.GetAsync(new GetModel<RefreshTkn>
        {
            Predicate = r => r.Token == refreshTkn.ReplacedByToken
        });

        if (childToken?.Revoked != null && childToken.Expires <= DateTime.UtcNow)
            await RevokeRefreshToken(childToken, ipAddress, reason, cancellationToken: cancellationToken);
        else
            await RevokeDescendantRefreshTokens(refreshTkn: childToken!, ipAddress, reason, cancellationToken);
    }

    public Task<RefreshTkn> CreateRefreshToken(User user, string ipAddress,CancellationToken cancellationToken)
    {
        var refreshToken = _tokenHelper.CreateRefreshToken(user, ipAddress);
        return Task.FromResult(refreshToken);
    }
}