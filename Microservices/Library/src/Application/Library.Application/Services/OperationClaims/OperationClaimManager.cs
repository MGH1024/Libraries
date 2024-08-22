using Domain;
using MGH.Core.Persistence.Models.Paging;
using Application.Features.OperationClaims.Rules;
using MGH.Core.Persistence.Models.Filters.GetModels;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Application.Services.OperationClaims;

public class OperationClaimManager(IUow uow, OperationClaimBusinessRules operationClaimBusinessRules)
    : IOperationClaimService
{
    public async Task<OperationClaim> GetAsync(GetModel<OperationClaim> getModel)
    {
        var operationClaim = await uow.OperationClaim.GetAsync(getModel);
        return operationClaim;
    }

    public async Task<IPaginate<OperationClaim>> GetListAsync(GetListAsyncModel<OperationClaim> getListAsyncModel)
    {
        var operationClaimList = await uow.OperationClaim.GetListAsync(getListAsyncModel);
        return operationClaimList;
    }

    public async Task<OperationClaim> AddAsync(OperationClaim operationClaim, CancellationToken cancellationToken)
    {
        await operationClaimBusinessRules.OperationClaimNameShouldNotExistWhenCreating(operationClaim.Name,
            cancellationToken);
        var addedOperationClaim = await uow.OperationClaim.AddAsync(operationClaim, cancellationToken);
        return addedOperationClaim;
    }

    public async Task<OperationClaim> UpdateAsync(OperationClaim operationClaim, CancellationToken cancellationToken)
    {
        await operationClaimBusinessRules.OperationClaimNameShouldNotExistWhenUpdating(operationClaim.Id,
            operationClaim.Name, cancellationToken);

        var updatedOperationClaim =
            await uow.OperationClaim.UpdateAsync(operationClaim, cancellationToken);

        return updatedOperationClaim;
    }

    public async Task<OperationClaim> DeleteAsync(OperationClaim operationClaim, bool permanent = false,
        CancellationToken cancellationToken = default)
    {
        var deletedOperationClaim =
            await uow.OperationClaim.DeleteAsync(operationClaim, cancellationToken: cancellationToken);
        return deletedOperationClaim;
    }
}