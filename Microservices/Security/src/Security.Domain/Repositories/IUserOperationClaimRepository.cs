using MGH.Core.Domain.Buses.Queries;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters.GetModels;
using MGH.Core.Persistence.Models.Paging;

namespace Domain.Repositories;

public interface IUserOperationClaimRepository : IQuery<UserOperationClaim>
{
    Task<UserOperationClaim> GetAsync(GetModel<UserOperationClaim> getBaseModel);

    Task<IPaginate<UserOperationClaim>> GetListAsync(GetListModelAsync<UserOperationClaim> getListAsyncModel);

    Task<IPaginate<UserOperationClaim>> GetDynamicListAsync(
        GetDynamicListModelAsync<UserOperationClaim> dynamicListAsyncModel);

    Task<UserOperationClaim> AddAsync(UserOperationClaim entity, CancellationToken cancellationToken);
    Task<UserOperationClaim> DeleteAsync(UserOperationClaim entity, bool permanent = false);
    Task<bool> AnyAsync(GetBaseModel<UserOperationClaim> @base);
    Task<UserOperationClaim> UpdateAsync(UserOperationClaim entity, CancellationToken cancellationToken);
    Task<IEnumerable<OperationClaim>> GetOperationClaim(User user,CancellationToken cancellationToken);
}