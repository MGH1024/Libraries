using MGH.Core.Domain.Outboxes;
using MGH.Core.Infrastructure.Persistence.EF.Base.Repository;

namespace Domain.Entities.Libraries;

public interface IOutBoxRepository : IRepository<OutboxMessage, Guid>
{
    OutboxMessage Update(OutboxMessage entity);
    void Update(IEnumerable<Guid> idList);
}