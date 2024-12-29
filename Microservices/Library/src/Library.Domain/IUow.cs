using Library.Domain.Books;
using Library.Domain.Lendings;
using Library.Domain.Libraries;
using Library.Domain.Members;
using Library.Domain.Outboxes;
using MGH.Core.Infrastructure.Persistence.Base;

namespace Library.Domain;

public interface IUow : IUnitOfWork
{
    IBookRepository Book { get; }
    IMemberRepository Member { get; }
    IOutBoxRepository OutBox { get; }
    ILibraryRepository Library { get; }
    ILendingRepository Lending { get; }
}