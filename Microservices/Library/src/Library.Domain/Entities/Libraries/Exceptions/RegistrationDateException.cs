using MGH.Core.CrossCutting.Exceptions.Types;

namespace Domain.Entities.Libraries.Exceptions;

public class RegistrationDateException() : LibraryException("library registration date in invalid");