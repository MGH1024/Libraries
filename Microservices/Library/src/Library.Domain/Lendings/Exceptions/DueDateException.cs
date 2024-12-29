using Library.Domain.Books.Exceptions;

namespace Library.Domain.Lendings.Exceptions;

public class DueDateException() : BorrowBookException("due date is invalid");