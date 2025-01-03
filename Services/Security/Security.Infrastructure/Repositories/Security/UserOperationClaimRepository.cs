using Microsoft.EntityFrameworkCore;
using Security.Infrastructure.Contexts;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Infrastructure.Persistence.EF.Base.Repository;
using Security.Domain.Repositories;

namespace Security.Infrastructure.Repositories.Security;

public class UserOperationClaimRepository(SecurityDbContext securityDbContext)
    : Repository<UserOperationClaim, int>(securityDbContext), IUserOperationClaimRepository
{
    public IQueryable<UserOperationClaim> Query() => securityDbContext.Set<UserOperationClaim>();

    public async Task<IEnumerable<OperationClaim>> GetOperationClaim(User user, CancellationToken cancellationToken)
    {
        var queryable =
            Query()
                .Where(p => p.UserId == user.Id)
                .Select(p => new OperationClaim { Id = p.OperationClaimId, Name = p.OperationClaim.Name });
        return await queryable.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<UserOperationClaim>> GetUserOperationClaims(int userId,
        CancellationToken cancellationToken)
    {
        var queryable = await Query().Where(p => p.UserId == userId).ToListAsync(cancellationToken);
        return queryable;
    }

    public async Task<OperationClaim> GetOperationClaimByUserAndOperationClaim(int userId, int operationClaimId,
        CancellationToken cancellationToken)
    {
        var queryable =
            Query()
                .Where(p => p.UserId == userId && p.OperationClaim.Id == operationClaimId)
                .Select(p => new OperationClaim { Id = p.OperationClaimId, Name = p.OperationClaim.Name });
        return await queryable.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<UserOperationClaim> GetOperationClaimByIdAndUserAndOperationClaim(int id, int userId,
        int operationClaimId,
        CancellationToken cancellationToken)
    {
        var queryable =
            Query()
                .Where(p => p.Id == id && p.UserId == userId && p.OperationClaim.Id == operationClaimId);
        return await queryable.FirstOrDefaultAsync(cancellationToken);
    }
}