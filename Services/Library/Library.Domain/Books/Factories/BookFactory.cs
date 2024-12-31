using Library.Domain.Books.ValueObjects;

namespace Library.Domain.Books.Factories;

public class BookFactory : IBookFactory
{
    public Book Create(Isbn isbn, Title title, PublicationDate publicationDate,
        UniqueCode uniqueCode, IsReference isReference)
    {
        return new Book(isbn, title, publicationDate,
            uniqueCode, isReference);
    }
}