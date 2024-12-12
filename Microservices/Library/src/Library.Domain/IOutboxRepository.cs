using MGH.Core.Domain.Entity.Outboxes;
using MGH.Core.Infrastructure.Persistence.Base;

namespace Library.Domain;

public interface IOutBoxRepository : IRepository<OutboxMessage, Guid>
{
    OutboxMessage Update(OutboxMessage entity);
    void Update(IEnumerable<Guid> idList);
}