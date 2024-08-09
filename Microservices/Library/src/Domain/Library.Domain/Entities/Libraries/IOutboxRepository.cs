using MGH.Core.Domain.Outboxes;
using MGH.Core.Persistence.Models.Filters;
using MGH.Core.Persistence.Models.Paging;


namespace Domain.Entities.Libraries;

public interface IOutBoxRepository 
{
    Task<OutboxMessage> GetAsync(GetBaseModel<OutboxMessage> getBaseModel);
    
    Task<IPaginate<OutboxMessage>> GetListAsync(GetListAsyncModel<OutboxMessage> getListAsyncModel);

    OutboxMessage Update(OutboxMessage entity);
    void Update(IEnumerable<Guid> idList);
}