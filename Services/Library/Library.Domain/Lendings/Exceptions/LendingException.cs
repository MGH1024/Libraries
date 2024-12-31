using MGH.Core.CrossCutting.Exceptions.Types;

namespace Library.Domain.Lendings.Exceptions;

public class LendingException(string message) : BusinessException(message);