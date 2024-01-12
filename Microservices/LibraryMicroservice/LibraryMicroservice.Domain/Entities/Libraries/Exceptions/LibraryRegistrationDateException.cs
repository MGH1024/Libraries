using MGH.Core.CrossCutting.Exceptions.Types;

namespace Domain.Entities.Libraries.Exceptions;

public class LibraryRegistrationDateException : LibraryException
{
    public LibraryRegistrationDateException() : base("library registration date in invalid")
    {
    }
}