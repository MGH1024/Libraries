using Domain.Entities.Libraries;
using MGH.Core.Infrastructure.Persistence.Persistence.Base;

namespace Domain;

public interface IUow: IUnitOfWork
{
    ILibraryRepository Library { get; }
    IOutBoxRepository OutBox { get; }
}