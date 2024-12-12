using Library.Domain.Entities.Books.ValueObjects;

namespace Library.Domain.Entities.Books.Factories;

public interface IBookFactory
{
    Book Create( Isbn isbn, Title title, PublicationDate publicationDate,
        UniqueCode uniqueCode, IsReference isReference);
}