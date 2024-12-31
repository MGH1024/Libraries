using MGH.Core.CrossCutting.Exceptions.Types;

namespace Library.Domain.Libraries.Exceptions;

public class LibraryException(string message) : BusinessException(message);