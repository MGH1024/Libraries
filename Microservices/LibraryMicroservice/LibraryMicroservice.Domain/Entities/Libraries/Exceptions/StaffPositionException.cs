using MGH.Core.CrossCutting.Exceptions.Types;

namespace Domain.Entities.Libraries.Exceptions;

public class StaffPositionException : LibraryException
{
    public StaffPositionException() : base("name is empty")
    {
    }
}