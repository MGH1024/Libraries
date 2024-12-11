using MGH.Core.Infrastructure.Persistence;
using MGH.Core.Infrastructure.Persistence.Base;
using MGH.Core.Infrastructure.Persistence.EF.Base.Repository;

namespace Domain.Entities.Libraries;

public interface ILibraryRepository : IAggregateRepository<Library, Guid>
{
}