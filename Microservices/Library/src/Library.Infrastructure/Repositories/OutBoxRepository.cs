using Persistence.Contexts;
using Domain.Entities.Libraries;
using MGH.Core.Infrastructure.Public;
using MGH.Core.Persistence.Base.Repository;
using MGH.Core.Domain.Outboxes;

namespace Persistence.Repositories;

public class OutBoxRepository(LibraryDbContext libraryDbContext, IDateTime dateTime)
    : Repository<OutboxMessage, Guid>(libraryDbContext), IOutBoxRepository
{
    public IQueryable<OutboxMessage> Query() => libraryDbContext.Set<OutboxMessage>();

    public OutboxMessage Update(OutboxMessage entity)
    {
        libraryDbContext.Update(entity);
        return entity;
    }

    public void Update(IEnumerable<Guid> idList)
    {
        var lstOutbox
            = Query().Where(a => idList.Contains(a.Id)).ToList();

        lstOutbox.ForEach(x => x.ProcessedAt = dateTime.IranNow);
    }
}