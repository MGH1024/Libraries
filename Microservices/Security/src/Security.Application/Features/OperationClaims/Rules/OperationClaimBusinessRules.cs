using Application.Features.OperationClaims.Constants;
using Domain;
using MGH.Core.Application.Rules;
using MGH.Core.CrossCutting.Exceptions.Types;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters.GetModels;

namespace Application.Features.OperationClaims.Rules;

public class OperationClaimBusinessRules(IUow uow) : BaseBusinessRules
{
    public Task OperationClaimShouldExistWhenSelected(OperationClaim operationClaim)
    {
        if (operationClaim == null)
            throw new BusinessException(OperationClaimsMessages.NotExists);
        return Task.CompletedTask;
    }

    public async Task OperationClaimIdShouldExistWhenSelected(int id, CancellationToken cancellationToken)
    {
        bool doesExist = await uow.OperationClaim.AnyAsync(
            new Base<OperationClaim>
            {
                Predicate = b => b.Id == id,
                CancellationToken = cancellationToken
            }, cancellationToken);
        if (doesExist)
            throw new BusinessException(OperationClaimsMessages.NotExists);
    }

    public async Task OperationClaimNameShouldNotExistWhenCreating(string name, CancellationToken cancellationToken)
    {
        var doesExist = await uow.OperationClaim.AnyAsync(new Base<OperationClaim>
        {
            Predicate = b => b.Name == name,
            CancellationToken = cancellationToken
        }, cancellationToken);
        if (doesExist)
            throw new BusinessException(OperationClaimsMessages.AlreadyExists);
    }

    public async Task OperationClaimNameShouldNotExistWhenUpdating(int id, string name,
        CancellationToken cancellationToken)
    {
        bool doesExist =
            await uow.OperationClaim.AnyAsync(
                new Base<OperationClaim>
                {
                    Predicate = b => b.Id != id && b.Name == name,
                    EnableTracking = false
                }, cancellationToken);
        if (doesExist)
            throw new BusinessException(OperationClaimsMessages.AlreadyExists);
    }
}