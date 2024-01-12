using MGH.Core.CrossCutting.Exceptions.Types;

namespace Domain.Entities.Libraries.Exceptions;

public class LibraryIdException: LibraryException
{
    public LibraryIdException():base("library id is invalid")
    {
        
    }
}