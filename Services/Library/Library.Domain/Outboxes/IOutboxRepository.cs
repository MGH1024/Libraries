using MGH.Core.Domain.Entities;
using MGH.Core.Infrastructure.Persistence.Base;

namespace Library.Domain.Outboxes;

public interface IOutBoxRepository : IRepository<OutboxMessage, Guid>
{
    OutboxMessage Update(OutboxMessage entity);
    void Update(IEnumerable<Guid> idList);
}