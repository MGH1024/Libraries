using MGH.Core.Infrastructure.Persistence.Models.Filters.GetModels;
using MGH.Core.Infrastructure.Persistence.Models.Paging;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Security.Application.Features.OperationClaims.Services;

public interface IOperationClaimService
{
    Task<OperationClaim> GetAsync(GetModel<OperationClaim> getModel);
    Task<IPaginate<OperationClaim>> GetListAsync(GetListModelAsync<OperationClaim> getListAsyncModel);
    Task<OperationClaim> AddAsync(OperationClaim operationClaim, CancellationToken cancellationToken);
    Task<OperationClaim> UpdateAsync(OperationClaim operationClaim, CancellationToken cancellationToken);

    Task<OperationClaim> DeleteAsync(OperationClaim operationClaim, bool permanent = false,
        CancellationToken cancellationToken = default);
}