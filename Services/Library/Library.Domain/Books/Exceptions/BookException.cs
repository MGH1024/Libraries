using MGH.Core.CrossCutting.Exceptions.ExceptionTypes;

namespace Library.Domain.Books.Exceptions;

public class BookException : BusinessException
{
    protected BookException(string message) : base(message)
    {
    }
}