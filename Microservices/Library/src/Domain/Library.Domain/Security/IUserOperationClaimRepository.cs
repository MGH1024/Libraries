using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters;
using MGH.Core.Persistence.Models.Paging;

namespace Domain.Security;

public interface IUserOperationClaimRepository
{
    Task<UserOperationClaim> GetAsync(GetBaseModel<UserOperationClaim> getBaseModel);

    Task<IPaginate<UserOperationClaim>> GetListAsync(GetListAsyncModel<UserOperationClaim> getListAsyncModel);

    Task<IPaginate<UserOperationClaim>> GetDynamicListAsync(
        GetDynamicListAsyncModel<UserOperationClaim> dynamicListAsyncModel);

    Task<UserOperationClaim> AddAsync(UserOperationClaim entity, CancellationToken cancellationToken);
    Task<UserOperationClaim> DeleteAsync(UserOperationClaim entity, bool permanent = false);
}