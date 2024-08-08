using System.Linq.Expressions;
using MGH.Core.Domain.Outboxes;
using MGH.Core.Infrastructure.Persistence.Persistence.Base;
using MGH.Core.Infrastructure.Persistence.Persistence.Models.Filters;
using MGH.Core.Infrastructure.Persistence.Persistence.Models.Paging;
using Microsoft.EntityFrameworkCore.Query;

namespace Domain.Entities.Libraries;

public interface IOutBoxRepository : IQuery<OutboxMessage>
{
    Task<OutboxMessage> GetAsync(GetBaseModel<OutboxMessage> getBaseModel);
    
    Task<IPaginate<OutboxMessage>> GetListAsync(GetListAsyncModel<OutboxMessage> getListAsyncModel);

    OutboxMessage Update(OutboxMessage entity);
    void Update(IEnumerable<Guid> idList);
}