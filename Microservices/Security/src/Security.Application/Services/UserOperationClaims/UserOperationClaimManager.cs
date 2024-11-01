﻿using Application.Features.UserOperationClaims.Rules;
using Domain.Repositories;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters.GetModels;
using MGH.Core.Persistence.Models.Paging;

namespace Application.Services.UserOperationClaims;

public class UserUserOperationClaimManager(
    IUserOperationClaimRepository userUserOperationClaimRepository,
    UserOperationClaimBusinessRules userOperationClaimBusinessRules) :
    IUserOperationClaimService
{
    public async Task<UserOperationClaim> GetAsync(GetModel<UserOperationClaim> getModel)
    {
        return await userUserOperationClaimRepository.GetAsync(getModel);
    }

    public async Task<IPaginate<UserOperationClaim>> GetListAsync(GetListModelAsync<UserOperationClaim> getListAsyncModel)
    {
        return await userUserOperationClaimRepository.GetListAsync(getListAsyncModel);
    }

    public async Task<UserOperationClaim> AddAsync(UserOperationClaim userUserOperationClaim, CancellationToken cancellationToken)
    {
        await userOperationClaimBusinessRules
            .UserShouldNotHasOperationClaimAlreadyWhenInsert(userUserOperationClaim.UserId, userUserOperationClaim.OperationClaimId,
                cancellationToken);
        return await userUserOperationClaimRepository.AddAsync(userUserOperationClaim, cancellationToken);
    }

    public async Task<UserOperationClaim> UpdateAsync(UserOperationClaim userUserOperationClaim,
        CancellationToken cancellationToken)
    {
        await userOperationClaimBusinessRules.UserShouldNotHasOperationClaimAlreadyWhenUpdated(
            userUserOperationClaim.Id,
            userUserOperationClaim.UserId,
            userUserOperationClaim.OperationClaimId,
            cancellationToken
        );
        return await userUserOperationClaimRepository.UpdateAsync(userUserOperationClaim, cancellationToken);
    }

    public async Task<UserOperationClaim> DeleteAsync(UserOperationClaim userUserOperationClaim, CancellationToken cancellationToken,
        bool permanent = false)
    {
        return await userUserOperationClaimRepository.DeleteAsync(userUserOperationClaim);
    }
}