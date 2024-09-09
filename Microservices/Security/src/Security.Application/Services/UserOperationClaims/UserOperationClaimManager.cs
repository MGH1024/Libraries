using Application.Features.UserOperationClaims.Rules;
using Domain.Entities.Security;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters.GetModels;
using MGH.Core.Persistence.Models.Paging;

namespace Application.Services.UserOperationClaims;

public class UserUserOperationClaimManager(
    IUserOperationClaimRepository userUserOperationClaimRepository,
    UserOperationClaimBusinessRules userUserOperationClaimBusinessRules)
    : IUserOperationClaimService
{
    private readonly UserOperationClaimBusinessRules _userUserOperationClaimBusinessRules = userUserOperationClaimBusinessRules;

    public async Task<UserOperationClaim> GetAsync(GetModel<UserOperationClaim> getModel)
    {
        var userUserOperationClaim = await userUserOperationClaimRepository.GetAsync(getModel);
        return userUserOperationClaim;
    }

    public async Task<IPaginate<UserOperationClaim>> GetListAsync(
        GetListAsyncModel<UserOperationClaim> getListAsyncModel)
    {
        var userUserOperationClaimList =
            await userUserOperationClaimRepository.GetListAsync(getListAsyncModel);
        return userUserOperationClaimList;
    }

    public async Task<UserOperationClaim> AddAsync(UserOperationClaim userUserOperationClaim,
        CancellationToken cancellationToken)
    {
        await _userUserOperationClaimBusinessRules.UserShouldNotHasOperationClaimAlreadyWhenInsert(
            userUserOperationClaim.UserId,
            userUserOperationClaim.OperationClaimId
        );

        var addedUserOperationClaim =
            await userUserOperationClaimRepository.AddAsync(userUserOperationClaim, cancellationToken);

        return addedUserOperationClaim;
    }

    public async Task<UserOperationClaim> UpdateAsync(UserOperationClaim userUserOperationClaim,
        CancellationToken cancellationToken)
    {
        await _userUserOperationClaimBusinessRules.UserShouldNotHasOperationClaimAlreadyWhenUpdated(
            userUserOperationClaim.Id,
            userUserOperationClaim.UserId,
            userUserOperationClaim.OperationClaimId
        );
        var updatedUserOperationClaim =
            await userUserOperationClaimRepository.UpdateAsync(userUserOperationClaim, cancellationToken);
        return updatedUserOperationClaim;
    }

    public async Task<UserOperationClaim> DeleteAsync(UserOperationClaim userUserOperationClaim,
        CancellationToken cancellationToken, bool permanent = false)
    {
        var deletedUserOperationClaim =
            await userUserOperationClaimRepository.DeleteAsync(userUserOperationClaim);
        return deletedUserOperationClaim;
    }
}