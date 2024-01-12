namespace Domain.Entities.Books.Exceptions;

public class DueDateException:BorrowBookException
{
    public DueDateException(): base("due date is invalid")
    {
    }
}