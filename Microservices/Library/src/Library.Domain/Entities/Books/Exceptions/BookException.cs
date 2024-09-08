using MGH.Core.CrossCutting.Exceptions.Types;

namespace Domain.Entities.Books.Exceptions;

public class BookException : BusinessException
{
    protected BookException(string message) : base(message)
    {
    }
}