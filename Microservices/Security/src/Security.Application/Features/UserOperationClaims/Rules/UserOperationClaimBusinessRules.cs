using Application.Features.UserOperationClaims.Constants;
using Domain.Repositories;
using MGH.Core.Application.Rules;
using MGH.Core.CrossCutting.Exceptions.Types;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters.GetModels;

namespace Application.Features.UserOperationClaims.Rules;

public class UserOperationClaimBusinessRules(IUserOperationClaimRepository userOperationClaimRepository)
    : BaseBusinessRules
{
    public Task UserOperationClaimShouldExistWhenSelected(UserOperationClaim userOperationClaim)
    {
        if (userOperationClaim is null)
            throw new BusinessException(UserOperationClaimsMessages.UserOperationClaimNotExists);
        return Task.CompletedTask;
    }

    public async Task UserOperationClaimIdShouldExistWhenSelected(int id)
    {
        var doesExist = await userOperationClaimRepository.AnyAsync(new GetModel<UserOperationClaim> { Predicate = b => b.Id == id });
        if (!doesExist)
            throw new BusinessException(UserOperationClaimsMessages.UserOperationClaimNotExists);
    }

    public Task UserOperationClaimShouldNotExistWhenSelected(UserOperationClaim userOperationClaim)
    {
        if (userOperationClaim is not null)
            throw new BusinessException(UserOperationClaimsMessages.UserOperationClaimAlreadyExists);
        return Task.CompletedTask;
    }

    public async Task UserShouldNotHasOperationClaimAlreadyWhenInsert(int userId, int operationClaimId)
    {
        var doesExist = await userOperationClaimRepository.AnyAsync(new GetBaseModel<UserOperationClaim>
        {
            Predicate = u => u.UserId == userId && u.OperationClaimId == operationClaimId
        });
        if (doesExist)
            throw new BusinessException(UserOperationClaimsMessages.UserOperationClaimAlreadyExists);
    }

    public async Task UserShouldNotHasOperationClaimAlreadyWhenUpdated(int id, int userId, int operationClaimId)
    {
        var doesExist = await userOperationClaimRepository.AnyAsync(
            new GetBaseModel<UserOperationClaim>
            {
                Predicate = uoc => uoc.Id == id && uoc.UserId == userId && uoc.OperationClaimId == operationClaimId
            });
        if (doesExist)
            throw new BusinessException(UserOperationClaimsMessages.UserOperationClaimAlreadyExists);
    }
}