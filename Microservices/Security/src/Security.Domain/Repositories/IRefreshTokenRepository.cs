using MGH.Core.Infrastructure.Persistence.EF.Base.Repository;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Domain.Repositories;

public interface IRefreshTokenRepository:IRepository<RefreshToken,int>
{
    Task<IEnumerable<RefreshToken>> GetRefreshTokenByUserId(int userId,int refreshTokenTtl, CancellationToken cancellationToken);
    Task DeleteRangeAsync(IEnumerable<RefreshToken> entities, bool permanent = false);
    Task<RefreshToken> GetByTokenAsync(string requestRefreshToken, CancellationToken cancellationToken);
}