using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters;
using MGH.Core.Persistence.Models.Filters.GetModels;
using MGH.Core.Persistence.Models.Paging;

namespace Domain.Security;

public interface IRefreshTokenRepository
{
    Task<RefreshTkn> GetAsync(GetModel<RefreshTkn> getBaseModel);

    Task<IPaginate<RefreshTkn>> GetListAsync(GetListAsyncModel<RefreshTkn> getListAsyncModel);

    Task<IPaginate<RefreshTkn>> GetDynamicListAsync(
        GetDynamicListAsyncModel<RefreshTkn> dynamicListAsyncModel);

    Task<RefreshTkn> AddAsync(RefreshTkn entity, CancellationToken cancellationToken);
    Task<RefreshTkn> DeleteAsync(RefreshTkn entity, bool permanent = false);
    IEnumerable<RefreshTkn> DeleteRange(IEnumerable<RefreshTkn> entity, bool permanent = false);
    Task<bool> AnyAsync(Base<RefreshTkn> @base);
    RefreshTkn Update(RefreshTkn entity,CancellationToken  cancellationToken);
    Task<IEnumerable<RefreshTkn>> GetRefreshTokenByUserId(int userId,int refreshTokenTtl, CancellationToken cancellationToken);
}