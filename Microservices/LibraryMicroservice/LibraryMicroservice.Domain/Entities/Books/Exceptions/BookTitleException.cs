namespace Domain.Entities.Books.Exceptions;

public class BookTitleException : BookException
{
    public BookTitleException() : base("book title is empty")
    {
    }
}