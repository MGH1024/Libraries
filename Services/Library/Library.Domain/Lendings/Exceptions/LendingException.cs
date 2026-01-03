using MGH.Core.CrossCutting.Exceptions.ExceptionTypes;

namespace Library.Domain.Lendings.Exceptions;

public class LendingException(string message) : BusinessException(message);