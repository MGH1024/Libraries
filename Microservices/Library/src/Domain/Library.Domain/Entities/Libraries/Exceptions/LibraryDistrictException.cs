using MGH.Core.CrossCutting.Exceptions.Types;

namespace Domain.Entities.Libraries.Exceptions;

public class LibraryDistrictException : LibraryException
{
    public LibraryDistrictException() : base("please enter true district")
    {
    }
}