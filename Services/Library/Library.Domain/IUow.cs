using Library.Domain.Books;
using Library.Domain.Members;
using Library.Domain.Outboxes;
using Library.Domain.Lendings;
using Library.Domain.Libraries;
using MGH.Core.Infrastructure.Persistence.Base;

namespace Library.Domain;

public interface IUow : IUnitOfWork
{
    IBookRepository Book { get; }
    IMemberRepository Member { get; }
    IOutboxMessageRepository OutBox { get; }
    IPublicLibraryRepository Library { get; }
    ILendingRepository Lending { get; }
}