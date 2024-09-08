using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters;
using MGH.Core.Persistence.Models.Filters.GetModels;
using MGH.Core.Persistence.Models.Paging;

namespace Domain.Security;

public interface IOperationClaimRepository
{
    Task<OperationClaim> GetAsync(GetModel<OperationClaim> getBaseModel);

    Task<IPaginate<OperationClaim>> GetListAsync(GetListAsyncModel<OperationClaim> getListAsyncModel);

    Task<IPaginate<OperationClaim>> GetDynamicListAsync(
        GetDynamicListAsyncModel<OperationClaim> dynamicListAsyncModel);

    Task<OperationClaim> AddAsync(OperationClaim entity, CancellationToken cancellationToken);

    Task<OperationClaim> DeleteAsync(OperationClaim entity, bool permanent = false,
        CancellationToken cancellationToken = default);

    Task<OperationClaim> UpdateAsync(OperationClaim entity, CancellationToken cancellationToken);
    Task<bool> AnyAsync(Base<OperationClaim> @base, CancellationToken cancellationToken);
}