using MGH.Core.Application.Rules;
using MGH.Core.CrossCutting.Exceptions.Types;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using Security.Application.Features.UserOperationClaims.Constants;
using Security.Domain.Repositories;

namespace Security.Application.Features.UserOperationClaims.Rules;

public class UserOperationClaimBusinessRules(IUserOperationClaimRepository userOperationClaimRepository) : BaseBusinessRules, IUserOperationClaimBusinessRules
{
    public Task UserOperationClaimShouldExistWhenSelected(UserOperationClaim userOperationClaim)
    {
        if (userOperationClaim is null)
            throw new BusinessException(UserOperationClaimsMessages.UserOperationClaimNotExists);
        return Task.CompletedTask;
    }

    public Task UserOperationClaimShouldNotExistWhenSelected(UserOperationClaim userOperationClaim)
    {
        if (userOperationClaim is not null)
            throw new BusinessException(UserOperationClaimsMessages.UserOperationClaimAlreadyExists);
        return Task.CompletedTask;
    }

    public async Task UserShouldNotHasOperationClaimAlreadyWhenInsert(int userId, int operationClaimId, CancellationToken cancellationToken)
    {
        var userOperationClaim =
            await userOperationClaimRepository.GetOperationClaimByUserAndOperationClaim(userId, operationClaimId, cancellationToken);
        if (userOperationClaim is not null)
            throw new BusinessException(UserOperationClaimsMessages.UserOperationClaimAlreadyExists);
    }

    public async Task UserShouldNotHasOperationClaimAlreadyWhenUpdated(int id, int userId, int operationClaimId, CancellationToken cancellationToken)
    {
        var userOperationClaim = await
            userOperationClaimRepository.GetOperationClaimByIdAndUserAndOperationClaim(id, userId, operationClaimId, cancellationToken);
        if (userOperationClaim is not null)
            throw new BusinessException(UserOperationClaimsMessages.UserOperationClaimAlreadyExists);
    }
}