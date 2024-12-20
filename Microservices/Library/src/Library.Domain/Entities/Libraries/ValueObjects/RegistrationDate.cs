﻿using Library.Domain.Entities.Libraries.Exceptions;
using MGH.Core.Domain.BaseEntity;

namespace Library.Domain.Entities.Libraries.ValueObjects;

public class RegistrationDate : ValueObject
{
    public DateTime Value { get; }
    
    public RegistrationDate(DateTime value)
    {
        var now = DateTime.Now.Date;
        if (value.Date >= now || value < now.AddYears(-100))
            throw new RegistrationDateException();
        Value = value;
    }

    public static implicit operator DateTime(RegistrationDate  registrationDate) => registrationDate.Value;
    public static implicit operator RegistrationDate(DateTime registrationDate) => new(registrationDate);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}