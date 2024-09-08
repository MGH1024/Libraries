using MGH.Core.Domain.Buses.Queries;
using MGH.Core.Persistence.Models.Paging;
using MGH.Core.Persistence.Models.Filters.GetModels;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Domain.Security;

public interface IUserOperationClaimRepository : IQuery<UserOperationClaim>
{
    Task<UserOperationClaim> GetAsync(GetModel<UserOperationClaim> getBaseModel);

    Task<IPaginate<UserOperationClaim>> GetListAsync(GetListAsyncModel<UserOperationClaim> getListAsyncModel);

    Task<IPaginate<UserOperationClaim>> GetDynamicListAsync(
        GetDynamicListAsyncModel<UserOperationClaim> dynamicListAsyncModel);

    Task<UserOperationClaim> AddAsync(UserOperationClaim entity, CancellationToken cancellationToken);
    Task<UserOperationClaim> DeleteAsync(UserOperationClaim entity, bool permanent = false);
    Task<bool> AnyAsync(Base<UserOperationClaim> @base);
    Task<UserOperationClaim> UpdateAsync(UserOperationClaim entity, CancellationToken cancellationToken);
    Task<IEnumerable<OperationClaim>> GetOperationClaim(User user,CancellationToken cancellationToken);
}