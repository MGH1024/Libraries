using Library.Domain.Entities.Libraries;
using Library.Infrastructure.Contexts;
using MGH.Core.Infrastructure.Persistence.EF.Base.Repository;

namespace Library.Infrastructure.Repositories;

public class LibraryRepository(LibraryDbContext libraryDbContext) :AggregateRepository<Domain.Entities.Libraries.Library,Guid>,ILibraryRepository
{
    
}