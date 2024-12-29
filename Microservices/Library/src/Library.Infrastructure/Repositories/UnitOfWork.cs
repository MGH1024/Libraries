using Library.Domain;
using Library.Domain.Lendings;
using Library.Domain.Libraries;
using Library.Domain.Outboxes;
using Library.Infrastructure.Contexts;
using MGH.Core.Infrastructure.Persistence.Base;

namespace Library.Infrastructure.Repositories;

public class UnitOfWork(
    LibraryDbContext context,
    IOutBoxRepository outBoxRepository,
    ILibraryRepository libraryRepository,
    ILendingRepository iLendingRepository,
    ITransactionManager<LibraryDbContext> transactionManager)
    : IUow, IDisposable
{
    public ILibraryRepository Library => libraryRepository;
    public IOutBoxRepository OutBox => outBoxRepository;

    public ILendingRepository Lending => iLendingRepository;

    public Task<int> CompleteAsync(CancellationToken cancellationToken)
    {
        return context.SaveChangesAsync(cancellationToken);
    }

    public Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return transactionManager.BeginTransactionAsync(cancellationToken);
    }

    public Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        return transactionManager.CommitTransactionAsync(cancellationToken);
    }

    public Task RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        return transactionManager.RollbackTransactionAsync(cancellationToken);
    }

    public void Dispose()
    {
        context.Dispose();
    }
}