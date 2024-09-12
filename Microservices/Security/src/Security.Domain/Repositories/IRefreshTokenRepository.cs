using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters.GetModels;
using MGH.Core.Persistence.Models.Paging;

namespace Domain.Repositories;

public interface IRefreshTokenRepository
{
    Task<RefreshTkn> GetAsync(GetModel<RefreshTkn> getBaseModel);

    Task<IPaginate<RefreshTkn>> GetListAsync(GetListModelAsync<RefreshTkn> getListAsyncModel);

    Task<IPaginate<RefreshTkn>> GetDynamicListAsync(GetDynamicListModelAsync<RefreshTkn> dynamicListAsyncModel);

    Task<bool> AnyAsync(GetBaseModel<RefreshTkn> @base);
    
    Task<RefreshTkn> AddAsync(RefreshTkn entity, CancellationToken cancellationToken);
    
    Task<RefreshTkn> DeleteAsync(RefreshTkn entity, bool permanent = false,CancellationToken cancellationToken=default);
    
    Task<IEnumerable<RefreshTkn>> DeleteRangeAsync(IEnumerable<RefreshTkn> entity, bool permanent = false,
        CancellationToken cancellationToken=default);
    
    Task<RefreshTkn> UpdateAsync(RefreshTkn entity, CancellationToken cancellationToken);
    
    Task<IEnumerable<RefreshTkn>> GetRefreshTokenByUserId(int userId,int refreshTokenTtl, CancellationToken cancellationToken);
}