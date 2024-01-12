namespace Domain.Entities.Books.Exceptions;

public class RegisterDateException : BorrowBookException
{
    public RegisterDateException():base("register date is invalid")
    {
        
    }
}