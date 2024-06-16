using System.Linq.Expressions;
using MGH.Core.Domain.Outboxes;
using MGH.Core.Infrastructure.Persistence.Base;
using MGH.Core.Infrastructure.Persistence.Models.Paging;
using Microsoft.EntityFrameworkCore.Query;

namespace Domain.Entities.Libraries;

public interface IOutBoxRepository : IQuery<OutboxMessage>
{
    Task<OutboxMessage> GetAsync(
        Expression<Func<OutboxMessage, bool>> predicate,
        Func<IQueryable<OutboxMessage>, IIncludableQueryable<OutboxMessage, object>> include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default);
    
    Task<IPaginate<OutboxMessage>> GetListAsync(
        Expression<Func<OutboxMessage, bool>> predicate = null,
        Func<IQueryable<OutboxMessage>, IOrderedQueryable<OutboxMessage>> orderBy = null,
        Func<IQueryable<OutboxMessage>, IIncludableQueryable<OutboxMessage, object>> include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    OutboxMessage Update(OutboxMessage entity);
    void Update(IEnumerable<Guid> idList);
}