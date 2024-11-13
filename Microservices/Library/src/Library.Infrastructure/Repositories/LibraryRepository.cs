using Domain.Entities.Libraries;
using MGH.Core.Infrastructure.Persistence.EF.Base.Repository;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class LibraryRepository(LibraryDbContext libraryDbContext) :AggregateRepository<Library,Guid>,ILibraryRepository
{
    
}