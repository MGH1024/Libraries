namespace Domain.Entities.Books.Exceptions;

public class IsbnInvalidLengthException:BookException
{
    public IsbnInvalidLengthException() : base("invalid isbn length")
    {
        
    }
}