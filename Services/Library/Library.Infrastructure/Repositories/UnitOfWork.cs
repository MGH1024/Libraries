using Library.Domain;
using Library.Domain.Books;
using Library.Domain.Lendings;
using Library.Domain.Libraries;
using Library.Domain.Members;
using Library.Domain.Outboxes;
using Library.Infrastructure.Contexts;

namespace Library.Infrastructure.Repositories;

public class UnitOfWork(
    PublicLibraryDbContext context,
    IOutboxMessageRepository outBoxRepository,
    IPublicLibraryRepository libraryRepository,
    ILendingRepository lendingRepository,
    IBookRepository bookRepository,
    IMemberRepository memberRepository)
    : IUow, IDisposable
{
    public IPublicLibraryRepository Library => libraryRepository;
    public IOutboxMessageRepository OutBox => outBoxRepository;
    public ILendingRepository Lending => lendingRepository;
    public IBookRepository Book => bookRepository;
    public IMemberRepository Member => memberRepository;

    public async Task<int> CompleteAsync(CancellationToken cancellationToken = default)
    {
        using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        int result = await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return result;
    }

    public void Dispose()
    {
        context.Dispose();
    }
}