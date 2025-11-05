using Library.Domain.Libraries;
using Library.Infrastructure.Contexts;
using MGH.Core.Infrastructure.Persistence.EF.Base.Repositories;

namespace Library.Infrastructure.Repositories;

public class LibraryRepository(LibraryDbContext libraryDbContext) :Repository<Domain.Libraries.Library,Guid>,ILibraryRepository
{
    
}