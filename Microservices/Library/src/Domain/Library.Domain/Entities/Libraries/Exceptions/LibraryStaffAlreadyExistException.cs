using MGH.Core.CrossCutting.Exceptions.Types;

namespace Domain.Entities.Libraries.Exceptions;

public class LibraryStaffAlreadyExistException() : LibraryException("staff already exist");