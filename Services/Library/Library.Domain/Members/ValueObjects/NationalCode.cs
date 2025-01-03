﻿using Library.Domain.Members.Exceptions;
using MGH.Core.Domain.BaseModels;

namespace Library.Domain.Members.ValueObjects;

public class NationalCode: ValueObject
{
    public string Value { get; }

    public NationalCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new MemberNationalCodeNullException();
        if (value.Length != 10)
            throw new MemberNationalCodeLengthException();
        Value = value;
    }

    public static implicit operator string(NationalCode nationalCode) => nationalCode.Value;
    public static implicit operator NationalCode(string nationalCode) => new(nationalCode);
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}