using MGH.Core.CrossCutting.Exceptions.Types;

namespace Domain.Entities.Libraries.Exceptions;

public class StaffNameException() : LibraryException("name is empty");