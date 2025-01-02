using System.Text.RegularExpressions;
using Library.Domain.Members.Exceptions;
using MGH.Core.Domain.BaseModels;

namespace Library.Domain.Members.ValueObjects;

public class MobileNumber : ValueObject
{
    public string Value { get; }

    public MobileNumber(string value)
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

    public static implicit operator string(MobileNumber mobileNumber) => mobileNumber.Value;
    public static implicit operator MobileNumber(string memberMobileNumber) => new(memberMobileNumber);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}