using MGH.Core.Domain.Outboxes;
using MGH.Core.Persistence.Models.Filters.GetModels;
using MGH.Core.Persistence.Models.Paging;


namespace Domain.Entities.Libraries;

public interface IOutBoxRepository 
{
    Task<OutboxMessage> GetAsync(GetModel<OutboxMessage> getBaseModel);
    
    Task<IPaginate<OutboxMessage>> GetListAsync(GetListAsyncModel<OutboxMessage> getListAsyncModel);

    OutboxMessage Update(OutboxMessage entity);
    void Update(IEnumerable<Guid> idList);
}