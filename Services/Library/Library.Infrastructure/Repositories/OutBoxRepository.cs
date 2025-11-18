using Library.Domain.Outboxes;
using MGH.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Library.Infrastructure.Contexts;

namespace Library.Infrastructure.Repositories;

public class OutBoxRepository(LibraryDbContext libraryDbContext) : IOutBoxRepository
{
    public IQueryable<OutboxMessage> Query() =>
        libraryDbContext.Set<OutboxMessage>();

    public async Task AddAsync(
        OutboxMessage message,
        CancellationToken cancellationToken = default)
    {
        await libraryDbContext.AddAsync(message, cancellationToken);
    }

    public async Task AddToOutBoxAsync(
        OutboxMessage message,
        CancellationToken cancellationToken = default)
    {
        await libraryDbContext.AddAsync(message, cancellationToken);
    }

    public async Task AddToOutBoxRangeAsync(
        IEnumerable<OutboxMessage> messages,
        CancellationToken cancellationToken = default)
    {
        await libraryDbContext.AddRangeAsync(messages, cancellationToken);
    }

    public async Task<IEnumerable<OutboxMessage>> GetListAsync(
        int pageIndex = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        return await Query()
            .Take(pageSize)
            .Skip(pageIndex * pageSize)
            .ToListAsync(cancellationToken);
    }

    public OutboxMessage Update(OutboxMessage entity)
    {
        libraryDbContext.Update(entity);
        return entity;
    }

    public async Task UpdateRangeAsync(
        IEnumerable<OutboxMessage> outboxMessages,
        CancellationToken cancellationToken = default)
    {
        libraryDbContext.UpdateRange(outboxMessages);
        await libraryDbContext.SaveChangesAsync(cancellationToken);
    }

}
