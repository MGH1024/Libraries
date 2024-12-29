using MGH.Core.Infrastructure.Persistence.Base;

namespace Library.Domain.Books;

public interface IBookRepository :IRepository<Book, Guid>
{
    
}