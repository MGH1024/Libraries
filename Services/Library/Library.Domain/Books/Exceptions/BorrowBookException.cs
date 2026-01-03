using MGH.Core.CrossCutting.Exceptions.ExceptionTypes;

namespace Library.Domain.Books.Exceptions;

public class BorrowBookException(string message) : BusinessException(message);