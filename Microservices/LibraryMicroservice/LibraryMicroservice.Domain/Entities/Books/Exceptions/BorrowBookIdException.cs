namespace Domain.Entities.Books.Exceptions;

public class BorrowBookIdException:BorrowBookException
{
    public BorrowBookIdException() : base("Id must be greater than zero")
    {
        
    }   
}