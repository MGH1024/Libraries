namespace Domain.Entities.Libraries.Exceptions;

public class LibraryLocationException:LibraryException
{
    public LibraryLocationException() : base("library location must not be empty")
    {
        
    }
}