using MGH.Core.CrossCutting.Exceptions.Types;

namespace Domain.Entities.Libraries.Exceptions;

public class LibraryNameException : LibraryException
{
    public LibraryNameException() : base("library name must not be empty")
    {
        
    }
}