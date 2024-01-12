using MGH.Core.CrossCutting.Exceptions.Types;
namespace Domain.Entities.Books.Exceptions;

public class BorrowBookException : BusinessException
{
    public BorrowBookException(string message):base(message)
    {
        
    }
}