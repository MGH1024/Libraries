using MGH.Core.CrossCutting.Exceptions.Types;

namespace Domain.Entities.Libraries.Exceptions;

public class LibraryIdException() : LibraryException("library id is invalid");