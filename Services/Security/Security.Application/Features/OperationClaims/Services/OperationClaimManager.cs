using MGH.Core.Infrastructure.Persistence.Models.Filters.GetModels;
using MGH.Core.Infrastructure.Persistence.Models.Paging;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using Security.Application.Features.OperationClaims.Rules;
using Security.Domain;

namespace Security.Application.Features.OperationClaims.Services;

public class OperationClaimManager(IUow uow, IOperationClaimBusinessRules operationClaimBusinessRules) : IOperationClaimService
{
    public async Task<OperationClaim> GetAsync(GetModel<OperationClaim> getModel)
    {
        var operationClaim = await uow.OperationClaim.GetAsync(getModel);
        return operationClaim;
    }

    public async Task<IPaginate<OperationClaim>> GetListAsync(GetListModelAsync<OperationClaim> getListAsyncModel)
    {
        var operationClaimList = await uow.OperationClaim.GetListAsync(getListAsyncModel);
        return operationClaimList;
    }

    public async Task<OperationClaim> AddAsync(OperationClaim operationClaim, CancellationToken cancellationToken)
    {
        await operationClaimBusinessRules.OperationClaimNameShouldNotExistWhenCreating(operationClaim.Name, cancellationToken);
        return await uow.OperationClaim.AddAsync(operationClaim, cancellationToken);
    }

    public async Task<OperationClaim> UpdateAsync(OperationClaim operationClaim, CancellationToken cancellationToken)
    {
        await operationClaimBusinessRules.OperationClaimNameShouldNotExistWhenUpdating(operationClaim.Id, operationClaim.Name, cancellationToken);
        return await uow.OperationClaim.UpdateAsync(operationClaim, cancellationToken);
    }

    public async Task<OperationClaim> DeleteAsync(OperationClaim operationClaim, bool permanent , CancellationToken cancellationToken )
    {
        return await uow.OperationClaim.DeleteAsync(operationClaim);
    }
}