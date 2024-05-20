using Domain.Entities.Members.Exceptions;

namespace Domain.Entities.Libraries.Exceptions;

public class StaffNationalCodeNullException : LibraryException
{
    public StaffNationalCodeNullException() : base("national code name must not be empty")
    {
    }
}