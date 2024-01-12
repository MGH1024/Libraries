using MGH.Core.CrossCutting.Exceptions.Types;

namespace Domain.Entities.Libraries.Exceptions;

public class LibraryException : BusinessException
{
    public LibraryException(string message) : base(message)
    {
    }
}