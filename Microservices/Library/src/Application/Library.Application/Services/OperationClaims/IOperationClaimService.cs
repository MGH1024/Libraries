using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters.GetModels;
using MGH.Core.Persistence.Models.Paging;

namespace Application.Services.OperationClaims;

public interface IOperationClaimService
{
    Task<OperationClaim> GetAsync(GetModel<OperationClaim> getModel);
    Task<IPaginate<OperationClaim>> GetListAsync(GetListAsyncModel<OperationClaim> getListAsyncModel);
    Task<OperationClaim> AddAsync(OperationClaim operationClaim, CancellationToken cancellationToken);
    Task<OperationClaim> UpdateAsync(OperationClaim operationClaim, CancellationToken cancellationToken);
    Task<OperationClaim> DeleteAsync(OperationClaim operationClaim, bool permanent = false);
}