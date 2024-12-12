using MGH.Core.Infrastructure.Persistence.Base;

namespace Library.Domain.Entities.Libraries;

public interface ILibraryRepository : IAggregateRepository<Library, Guid>
{
}