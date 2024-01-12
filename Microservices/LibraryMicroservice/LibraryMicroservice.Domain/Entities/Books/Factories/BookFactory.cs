using Domain.Entities.Books.ValueObjects;

namespace Domain.Entities.Books.Factories;

public class BookFactory : IBookFactory
{
    public Book Create(BookIsbn bookIsbn, BookTitle bookTitle, BookPublicationDate bookPublicationDate,
        BookUniqueCode bookUniqueCode, BookIsReference bookIsReference)
    {
        return new Book(bookIsbn, bookTitle, bookPublicationDate,
            bookUniqueCode, bookIsReference);
    }
}