using Domain.Entities.Libraries;
using MGH.Core.Persistence.Base.Repository;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class LibraryRepository(LibraryDbContext libraryDbContext) :AggregateRepository<Library,Guid>,ILibraryRepository
{
    
}