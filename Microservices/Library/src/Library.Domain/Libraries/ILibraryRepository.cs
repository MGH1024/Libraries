using MGH.Core.Infrastructure.Persistence.Base;

namespace Library.Domain.Libraries;

public interface ILibraryRepository : IAggregateRepository<Library, Guid>
{
    
}