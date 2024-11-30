using MGH.Core.Infrastructure.Persistence.EF.Base.Repository;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Domain.Repositories;

public interface IRefreshTokenRepository:IRepository<RefreshTkn,int>
{
    Task<IEnumerable<RefreshTkn>> GetRefreshTokenByUserId(int userId,int refreshTokenTtl, CancellationToken cancellationToken);
    Task DeleteRangeAsync(IEnumerable<RefreshTkn> entities, bool permanent = false);
    Task<RefreshTkn> GetByTokenAsync(string requestRefreshToken, CancellationToken cancellationToken);
}