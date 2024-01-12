namespace Domain.Entities.Books.Exceptions;

public class BookAuthorAlreadyExistException : BookException
{
    public BookAuthorAlreadyExistException():base("author already exist")
    {
        
    }
}