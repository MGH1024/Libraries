namespace Library.Domain.Books.Exceptions;

public class BorrowBookIdException() : BorrowBookException("Id must be greater than zero");