namespace Domain.Entities.Books.Exceptions;

public class CopyAlreadyExistException : BookException
{
    public CopyAlreadyExistException():base("copy already exist")
    {
        
    }
}