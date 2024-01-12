namespace Domain.Entities.Books.Exceptions;

public class BookPublicationDateException : BookException
{
    public BookPublicationDateException() : base("book publication date is invalid")
    {
    }
}