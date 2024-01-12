using System.Text.RegularExpressions;
using Domain.Entities.Members.Exceptions;

using MGH.Core.Domain.Concretes;

namespace Domain.Entities.Members.ValueObjects;

public class MemberMobileNumber : ValueObject
{
    public string Value { get; }

    public MemberMobileNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new MemberMobileNumberNullException();

        if (value.Length != 11)
            throw new MemberMobileNumberLengthException();

        if (Regex.IsMatch(value, @"^\d+$"))
            throw new MemberMobileNumberNotNumberException();

        if (!value.StartsWith("09"))
            throw new MemberMobileNumberFormatException();

        Value = value;
    }

    public static implicit operator string(MemberMobileNumber memberMobileNumber) => memberMobileNumber.Value;
    public static implicit operator MemberMobileNumber(string memberMobileNumber) => new(memberMobileNumber);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}