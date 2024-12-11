using MGH.Core.Infrastructure.Persistence;
using MGH.Core.Infrastructure.Persistence.Base;
using MGH.Core.Infrastructure.Persistence.EF.Base.Repository;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Domain.Repositories;

public interface IOperationClaimRepository: IRepository<OperationClaim, int>
{
   
}