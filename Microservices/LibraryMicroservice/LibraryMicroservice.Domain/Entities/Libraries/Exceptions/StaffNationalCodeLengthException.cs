﻿using Domain.Entities.Members.Exceptions;

namespace Domain.Entities.Libraries.Exceptions;

public class StaffNationalCodeLengthException : LibraryException
{
    public StaffNationalCodeLengthException():base("length of national code not equal to 10")
    {
        
    }
}