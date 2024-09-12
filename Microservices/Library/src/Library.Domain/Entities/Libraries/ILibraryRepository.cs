using MGH.Core.Persistence.Base.Repository;

namespace Domain.Entities.Libraries;

public interface ILibraryRepository : IAggregateRepository<Library, Guid>
{
}