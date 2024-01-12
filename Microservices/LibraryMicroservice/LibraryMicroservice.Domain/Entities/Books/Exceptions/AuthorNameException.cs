namespace Domain.Entities.Books.Exceptions;

public class AuthorNameException :BookException
{
    public AuthorNameException() : base("author name is empty")
    {
    }
}