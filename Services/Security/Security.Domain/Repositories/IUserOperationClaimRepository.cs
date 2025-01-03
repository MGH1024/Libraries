using MGH.Core.Infrastructure.Persistence.Base;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Security.Domain.Repositories;

public interface IUserOperationClaimRepository : IRepository<UserOperationClaim, int>
{
    Task<IEnumerable<OperationClaim>> GetOperationClaim(User user,CancellationToken cancellationToken);
    Task<IEnumerable<UserOperationClaim>> GetUserOperationClaims(int userId, CancellationToken cancellationToken);

    Task<OperationClaim> GetOperationClaimByUserAndOperationClaim(int userId, int operationClaimId,
        CancellationToken cancellationToken);
    
    Task<UserOperationClaim> GetOperationClaimByIdAndUserAndOperationClaim(int id,int userId, int operationClaimId,
        CancellationToken cancellationToken);
}