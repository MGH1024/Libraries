using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Infrastructure.Securities.Security.JWT;

namespace Application.Features.Auth.Services;

public interface IAuthService
{
    public Task<AccessToken> CreateAccessTokenAsync(User user, CancellationToken cancellationToken);
    public Task<RefreshTkn> CreateRefreshToken(User user);
    public Task<RefreshTkn> AddRefreshTokenAsync(RefreshTkn refreshTkn, CancellationToken cancellationToken);
    public Task DeleteOldRefreshTokens(int userId, CancellationToken cancellationToken);
    public Task<RefreshTkn> RotateRefreshToken(User user, RefreshTkn refreshTkn, CancellationToken cancellationToken);
    Task RevokeDescendantRefreshTokens(RefreshTkn refreshTkn, string reason, CancellationToken cancellationToken);
    void SetHashPassword(string password, User user);
}