using Domain.Entities.Libraries.ValueObjects;
using MGH.Core.CrossCutting.Exceptions.Types;

namespace Domain.Entities.Libraries.Exceptions;

public class LibraryStaffNotFoundException() : LibraryException("library staff not found");