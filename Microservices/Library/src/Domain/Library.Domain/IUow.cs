using Domain.Entities.Libraries;
using MGH.Core.Persistence.Base;

namespace Domain;

public interface IUow: IUnitOfWork
{
    ILibraryRepository Library { get; }
    IOutBoxRepository OutBox { get; }
}