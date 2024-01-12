namespace Domain.Entities.Books.Exceptions;

public class BookIdException : BookException
{
    public BookIdException():base("invalid book id")
    {
        
    }
}