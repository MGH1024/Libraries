using MGH.Core.Infrastructure.Persistence.UnitOfWork;
using Persistence.Contexts;

namespace Persistence;

public class UnitOfWork(LibraryDbContext context) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        context.Dispose();
    }
}