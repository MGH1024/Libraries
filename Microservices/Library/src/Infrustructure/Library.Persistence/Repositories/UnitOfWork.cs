using Domain;
using Domain.Entities.Libraries;
using MGH.Core.Infrastructure.Public;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class UnitOfWork(LibraryDbContext context, IDateTime dateTime) : IUow
{
    private IDbContextTransaction _transaction;
    private LibraryRepository _libraryRepository;
    private OutBoxRepository _outBoxRepository;

    public ILibraryRepository Library => _libraryRepository ??= new LibraryRepository(context);
    public IOutBoxRepository OutBox => _outBoxRepository ??= new OutBoxRepository(context, dateTime);


    public async Task<int> CompleteAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        _transaction = await context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        try
        {
            await context.SaveChangesAsync(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            await _transaction.DisposeAsync();
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _transaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            await _transaction.DisposeAsync();
        }
    }

    public void Dispose()
    {
        context.Dispose();
    }
}