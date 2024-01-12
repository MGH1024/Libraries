﻿using MGH.Core.CrossCutting.Exceptions.Types;

namespace Domain.Entities.Libraries.Exceptions;

public class StaffNameException : LibraryException
{
    public StaffNameException() : base("name is empty")
    {
    }
}