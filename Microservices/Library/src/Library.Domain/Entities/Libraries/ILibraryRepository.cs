using MGH.Core.Infrastructure.Persistence.EF.Base.Repository;

namespace Domain.Entities.Libraries;

public interface ILibraryRepository : IAggregateRepository<Library, Guid>
{
}