using Library.Domain.Lendings;
using Library.Domain.Libraries;
using Library.Domain.Outboxes;
using MGH.Core.Infrastructure.Persistence.Base;

namespace Library.Domain;

public interface IUow : IUnitOfWork
{
    ILibraryRepository Library { get; }
    IOutBoxRepository OutBox { get; }
    ILendingRepository Lending { get; }
}