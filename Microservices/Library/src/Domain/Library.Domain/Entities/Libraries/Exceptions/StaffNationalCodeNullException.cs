using Domain.Entities.Members.Exceptions;

namespace Domain.Entities.Libraries.Exceptions;

public class StaffNationalCodeNullException() : LibraryException("national code name must not be empty");