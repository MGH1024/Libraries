using MGH.Core.Domain.Entities;
using MGH.Core.Infrastructure.EventBus;

namespace Library.Domain.Outboxes;

public interface IOutBoxRepository : IOutboxStore
{
    Task<IEnumerable<OutboxMessage>> GetListAsync(
        int pageIndex = 1,
        int pageSize = 100,
        CancellationToken cancellationToken = default);

    Task UpdateRangeAsync(
         IEnumerable<OutboxMessage> outboxMessages,
         CancellationToken cancellationToken = default);

    OutboxMessage Update(OutboxMessage entity);
}
