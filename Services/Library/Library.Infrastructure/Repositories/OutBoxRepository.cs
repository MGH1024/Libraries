using Library.Domain;
using Library.Domain.Outboxes;
using Library.Infrastructure.Contexts;
using MGH.Core.Domain.Entity.Outboxes;
using MGH.Core.Infrastructure.Persistence.EF.Base.Repository;
using MGH.Core.Infrastructure.Public;

namespace Library.Infrastructure.Repositories;

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