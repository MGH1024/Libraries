using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Base.Repository;
using MGH.Core.Persistence.Models.Filters.GetModels;
using MGH.Core.Persistence.Models.Paging;

namespace Domain.Repositories;

public interface IRefreshTokenRepository:IRepository<RefreshTkn,int>
{
    Task<IEnumerable<RefreshTkn>> GetRefreshTokenByUserId(int userId,int refreshTokenTtl, CancellationToken cancellationToken);
    Task DeleteRangeAsync(IEnumerable<RefreshTkn> entities, bool permanent = false);
}