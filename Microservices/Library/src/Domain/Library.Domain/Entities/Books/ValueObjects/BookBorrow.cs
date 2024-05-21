﻿using MGH.Core.Domain.Concretes;

namespace Domain.Entities.Books.ValueObjects;

public class BookBorrow : ValueObject
{
    public bool Value { get; }

    public BookBorrow(bool value)
    {
        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}