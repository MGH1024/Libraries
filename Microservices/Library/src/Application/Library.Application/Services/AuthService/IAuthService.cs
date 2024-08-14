using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Infrastructure.Securities.Security.JWT;

namespace Application.Services.AuthService;

public interface IAuthService
{
    public Task<AccessToken> CreateAccessToken(User user, CancellationToken cancellationToken);
    public Task<RefreshTkn> CreateRefreshToken(User user, string ipAddress, CancellationToken cancellationToken);
    public Task<RefreshTkn> GetRefreshTokenByToken(string token, CancellationToken cancellationToken);
    public Task<RefreshTkn> AddRefreshToken(RefreshTkn refreshTkn, CancellationToken cancellationToken);
    public Task DeleteOldRefreshTokens(int userId, CancellationToken cancellationToken);

    public Task RevokeDescendantRefreshTokens(RefreshTkn refreshTkn, string ipAddress,CancellationToken cancellationToken,
        string reason);

    public Task RevokeRefreshToken(RefreshTkn tkn, string ipAddress, CancellationToken cancellationToken,
        string reason = null, string replacedByToken = null);

    public Task<RefreshTkn> RotateRefreshToken(User user, RefreshTkn refreshTkn, string ipAddress,
        CancellationToken cancellationToken);
}