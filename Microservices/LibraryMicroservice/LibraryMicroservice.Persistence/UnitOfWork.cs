using MGH.Core.Persistence.UnitOfWork;
using Persistence.Contexts;

namespace Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly LibraryDbContext _context;
    public UnitOfWork(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}