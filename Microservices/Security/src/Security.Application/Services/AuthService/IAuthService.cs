using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Infrastructure.Securities.Security.JWT;

namespace Application.Services.AuthService;

public interface IAuthService
{
    public Task<AccessToken> CreateAccessTokenAsync(User user, CancellationToken cancellationToken);
    public Task<RefreshTkn> CreateRefreshToken(User user, CancellationToken cancellationToken);
    public Task<RefreshTkn> GetRefreshTokenByToken(string token, CancellationToken cancellationToken);
    public Task<RefreshTkn> AddRefreshTokenAsync(RefreshTkn refreshTkn, CancellationToken cancellationToken);
    public Task DeleteOldRefreshTokens(int userId, CancellationToken cancellationToken);

    Task RevokeRefreshTokenAsync(RefreshTkn refreshToken, string reason = null, string replacedByToken = null,
        CancellationToken cancellationToken = default);

    public Task<RefreshTkn> RotateRefreshToken(User user, RefreshTkn refreshTkn, CancellationToken cancellationToken);

    Task RevokeDescendantRefreshTokens(RefreshTkn refreshTkn, string reason, CancellationToken cancellationToken);
}