namespace Domain.Entities.Books.Exceptions;

public class BookIsbnException : BookException
{
    public BookIsbnException() : base("isbn is empty")
    {
    }
}