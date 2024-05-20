using Domain.Entities.Books.ValueObjects;

namespace Domain.Entities.Books.Factories;

public interface IBookFactory
{
    Book Create( BookIsbn bookIsbn, BookTitle bookTitle, BookPublicationDate bookPublicationDate,
        BookUniqueCode bookUniqueCode, BookIsReference bookIsReference);
}