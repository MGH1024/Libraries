using MGH.Core.CrossCutting.Exceptions.Types;

namespace Domain.Entities.Libraries.Exceptions;

public class LibraryCodeNullException : LibraryException
{
    public LibraryCodeNullException():base("library code is null")
    {
        
    }
}