using MGH.Core.CrossCutting.Exceptions.Types;

namespace Library.Domain.Entities.Libraries.Exceptions;

public class LibraryException(string message) : BusinessException(message);