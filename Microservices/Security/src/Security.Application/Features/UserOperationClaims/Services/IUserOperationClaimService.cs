using MGH.Core.Infrastructure.Persistence.EF.Models.Filters.GetModels;
using MGH.Core.Infrastructure.Persistence.EF.Models.Paging;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Application.Features.UserOperationClaims.Services;

public interface IUserOperationClaimService
{
    Task<UserOperationClaim> GetAsync(GetModel<UserOperationClaim> getModel);
    Task<IPaginate<UserOperationClaim>> GetListAsync(GetListModelAsync<UserOperationClaim> getListAsyncModel);
    Task<UserOperationClaim> AddAsync(UserOperationClaim userOperationClaim,CancellationToken cancellationToken);
    Task<UserOperationClaim> UpdateAsync(UserOperationClaim userOperationClaim,CancellationToken cancellationToken);
    Task<UserOperationClaim> DeleteAsync(UserOperationClaim userOperationClaim,CancellationToken cancellationToken, bool permanent = false);
}
