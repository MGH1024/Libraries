using MGH.Core.CrossCutting.Exceptions.Types;

namespace Domain.Entities.Libraries.Exceptions;

public class LibraryStaffAlreadyExistException : LibraryException
{
    public LibraryStaffAlreadyExistException():base("staff already exist")
    {
        
    }
}