using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters;
using MGH.Core.Persistence.Models.Filters.GetModels;
using MGH.Core.Persistence.Models.Paging;

namespace Domain.Security;

public interface IRefreshTokenRepository
{
    Task<RefreshToken> GetAsync(GetModel<RefreshToken> getBaseModel);

    Task<IPaginate<RefreshToken>> GetListAsync(GetListAsyncModel<RefreshToken> getListAsyncModel);

    Task<IPaginate<RefreshToken>> GetDynamicListAsync(
        GetDynamicListAsyncModel<RefreshToken> dynamicListAsyncModel);

    Task<RefreshToken> AddAsync(RefreshToken entity, CancellationToken cancellationToken);
    Task<RefreshToken> DeleteAsync(RefreshToken entity, bool permanent = false);
}