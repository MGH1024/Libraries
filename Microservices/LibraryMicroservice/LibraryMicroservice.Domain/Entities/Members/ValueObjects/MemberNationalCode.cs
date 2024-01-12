using Domain.Entities.Members.Exceptions;

using MGH.Core.Domain.Concretes;

namespace Domain.Entities.Members.ValueObjects;

public class MemberNationalCode: ValueObject
{
    public string Value { get; }

    public MemberNationalCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new MemberNationalCodeNullException();
        if (value.Length != 10)
            throw new MemberNationalCodeLengthException();
        Value = value;
    }

    public static implicit operator string(MemberNationalCode nationalCode) => nationalCode.Value;
    public static implicit operator MemberNationalCode(string nationalCode) => new(nationalCode);
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}