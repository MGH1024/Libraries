using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Infrastructure.Securities.Security.JWT;

namespace Security.Application.Features.Auth.Services;

public interface IAuthService
{
    public Task<AccessToken> CreateAccessTokenAsync(User user, CancellationToken cancellationToken);
    public Task<RefreshToken> CreateRefreshToken(User user);
    public Task<RefreshToken> AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
    public Task DeleteOldRefreshTokens(int userId, CancellationToken cancellationToken);
    public Task<RefreshToken> RotateRefreshToken(User user, RefreshToken refreshToken, CancellationToken cancellationToken);
    Task RevokeDescendantRefreshTokens(RefreshToken refreshToken, string reason, CancellationToken cancellationToken);
    void SetHashPassword(string password, User user);
}