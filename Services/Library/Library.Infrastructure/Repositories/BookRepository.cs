using Library.Domain.Books;
using Library.Infrastructure.Contexts;
using MGH.Core.Infrastructure.Persistence.EF.Base.Repository;

namespace Library.Infrastructure.Repositories;

public class BookRepository(LibraryDbContext libraryDbContext) :Repository<Book,Guid>,IBookRepository
{
    
}