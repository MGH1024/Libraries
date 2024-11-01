﻿using MGH.Core.Domain.Aggregate;

namespace Domain.Entities.Books.ValueObjects;

public class Borrow : ValueObject
{
    public bool Value { get; }

    public Borrow(bool value)
    {
        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}