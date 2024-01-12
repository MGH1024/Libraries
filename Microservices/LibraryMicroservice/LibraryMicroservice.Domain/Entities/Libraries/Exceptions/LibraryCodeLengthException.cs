using MGH.Core.CrossCutting.Exceptions.Types;

namespace Domain.Entities.Libraries.Exceptions;

public class LibraryCodeLengthException : LibraryException
{
    public LibraryCodeLengthException() : base("invalid library code length")
    {
    }
}