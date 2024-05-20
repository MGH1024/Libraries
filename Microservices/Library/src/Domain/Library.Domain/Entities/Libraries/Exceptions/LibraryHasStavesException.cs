using MGH.Core.CrossCutting.Exceptions.Types;

namespace Domain.Entities.Libraries.Exceptions;

public class LibraryHasStavesException : LibraryException
{
    public LibraryHasStavesException() : base("library has some staves")
    {
    }
}