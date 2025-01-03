using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Security.Application.Features.OperationClaims.Rules;

public interface IOperationClaimBusinessRules
{
    Task OperationClaimShouldExistWhenSelected(OperationClaim operationClaim);
    Task OperationClaimIdShouldExistWhenSelected(int id, CancellationToken cancellationToken);
    Task OperationClaimNameShouldNotExistWhenCreating(string name, CancellationToken cancellationToken);

    Task OperationClaimNameShouldNotExistWhenUpdating(int id, string name, CancellationToken cancellationToken);
}