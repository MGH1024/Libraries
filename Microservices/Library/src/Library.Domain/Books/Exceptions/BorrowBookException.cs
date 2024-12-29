using MGH.Core.CrossCutting.Exceptions.Types;

namespace Library.Domain.Books.Exceptions;

public class BorrowBookException(string message) : BusinessException(message);