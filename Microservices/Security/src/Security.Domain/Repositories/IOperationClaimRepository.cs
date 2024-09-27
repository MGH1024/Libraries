using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Base.Repository;
using MGH.Core.Persistence.Models.Filters.GetModels;
using MGH.Core.Persistence.Models.Paging;

namespace Domain.Repositories;

public interface IOperationClaimRepository: IRepository<OperationClaim, int>
{
   
}