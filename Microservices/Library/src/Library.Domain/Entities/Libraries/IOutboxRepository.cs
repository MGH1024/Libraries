using MGH.Core.Domain.Entity.Outboxes;
using MGH.Core.Infrastructure.Persistence;
using MGH.Core.Infrastructure.Persistence.Base;
using MGH.Core.Infrastructure.Persistence.EF.Base.Repository;

namespace Domain.Entities.Libraries;

public interface IOutBoxRepository : IRepository<OutboxMessage, Guid>
{
    OutboxMessage Update(OutboxMessage entity);
    void Update(IEnumerable<Guid> idList);
}