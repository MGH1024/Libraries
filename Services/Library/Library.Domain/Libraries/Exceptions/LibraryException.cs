using MGH.Core.CrossCutting.Exceptions.ExceptionTypes;

namespace Library.Domain.Libraries.Exceptions;

public class LibraryException(string message) : BusinessException(message);