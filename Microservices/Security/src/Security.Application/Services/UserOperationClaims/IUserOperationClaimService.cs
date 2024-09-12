using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters.GetModels;
using MGH.Core.Persistence.Models.Paging;

namespace Application.Services.UserOperationClaims;

public interface IUserOperationClaimService
{
    Task<UserOperationClaim> GetAsync(GetModel<UserOperationClaim> getModel);
    Task<IPaginate<UserOperationClaim>> GetListAsync(GetListModelAsync<UserOperationClaim> getListAsyncModel);
    Task<UserOperationClaim> AddAsync(UserOperationClaim userOperationClaim,CancellationToken cancellationToken);
    Task<UserOperationClaim> UpdateAsync(UserOperationClaim userOperationClaim,CancellationToken cancellationToken);
    Task<UserOperationClaim> DeleteAsync(UserOperationClaim userOperationClaim,CancellationToken cancellationToken, bool permanent = false);
}
