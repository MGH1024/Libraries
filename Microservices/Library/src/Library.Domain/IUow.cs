using Library.Domain.Entities.Libraries;
using MGH.Core.Infrastructure.Persistence.Base;

namespace Library.Domain;

public interface IUow : IUnitOfWork
{
    ILibraryRepository Library { get; }
    IOutBoxRepository OutBox { get; }
}