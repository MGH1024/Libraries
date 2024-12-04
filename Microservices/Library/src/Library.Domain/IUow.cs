using Domain.Entities.Libraries;
using MGH.Core.Domain;

namespace Domain;

public interface IUow : IUnitOfWork
{
    ILibraryRepository Library { get; }
    IOutBoxRepository OutBox { get; }
}