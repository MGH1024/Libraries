using Library.Domain.Books;
using Library.Infrastructure.Contexts;
using MGH.Core.Infrastructure.Persistence.EF.Base.Repositories;

namespace Library.Infrastructure.Repositories;


public class BookRepository(PublicLibraryDbContext libraryDbContext)
    : Repository<Book, Guid>(libraryDbContext), IBookRepository
{
}
