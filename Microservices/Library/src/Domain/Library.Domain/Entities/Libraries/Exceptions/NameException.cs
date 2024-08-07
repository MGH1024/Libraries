using MGH.Core.CrossCutting.Exceptions.Types;

namespace Domain.Entities.Libraries.Exceptions;

public class NameException() : LibraryException("library name must not be empty");