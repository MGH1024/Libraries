using Domain.Entities.Libraries;
using MGH.Core.Domain;
using MGH.Core.Infrastructure.Persistence;
using MGH.Core.Infrastructure.Persistence.Base;

namespace Domain;

public interface IUow : IUnitOfWork
{
    ILibraryRepository Library { get; }
    IOutBoxRepository OutBox { get; }
}