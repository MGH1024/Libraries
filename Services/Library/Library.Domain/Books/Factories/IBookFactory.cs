using Library.Domain.Books.ValueObjects;

namespace Library.Domain.Books.Factories;

public interface IBookFactory
{
    Book Create( Isbn isbn, Title title, PublicationDate publicationDate,
        UniqueCode uniqueCode, IsReference isReference);
}