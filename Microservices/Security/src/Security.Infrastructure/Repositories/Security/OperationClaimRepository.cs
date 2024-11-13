using Domain.Repositories;
using MGH.Core.Infrastructure.Persistence.EF.Base.Repository;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories.Security;

public class OperationClaimRepository(SecurityDbContext securityDbContext) : Repository<OperationClaim, int>(securityDbContext),
    IOperationClaimRepository
{
    public IQueryable<OperationClaim> Query() => securityDbContext.Set<OperationClaim>();
}