using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Security.Application.Features.UserOperationClaims.Rules;

public interface IUserOperationClaimBusinessRules
{
    Task UserOperationClaimShouldExistWhenSelected(UserOperationClaim userOperationClaim);
    Task UserOperationClaimShouldNotExistWhenSelected(UserOperationClaim userOperationClaim);
    Task UserShouldNotHasOperationClaimAlreadyWhenInsert(int userId, int operationClaimId, CancellationToken cancellationToken);
    Task UserShouldNotHasOperationClaimAlreadyWhenUpdated(int id, int userId, int operationClaimId, CancellationToken cancellationToken);
}