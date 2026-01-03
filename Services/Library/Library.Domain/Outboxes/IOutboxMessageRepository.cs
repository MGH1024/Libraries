using MGH.Core.Infrastructure.Persistence;
using MGH.Core.Infrastructure.Persistence.Entities;

namespace Library.Domain.Outboxes;

public interface IOutboxMessageRepository : IOutboxStore
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
