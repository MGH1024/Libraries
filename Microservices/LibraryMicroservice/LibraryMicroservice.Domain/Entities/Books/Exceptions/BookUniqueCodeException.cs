namespace Domain.Entities.Books.Exceptions;

public class BookUniqueCodeException : BookException
{
    public BookUniqueCodeException() : base("unique code is empty")
    {
    }
}