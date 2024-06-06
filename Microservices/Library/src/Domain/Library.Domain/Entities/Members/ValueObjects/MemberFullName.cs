using Domain.Entities.Members.Exceptions;
using MGH.Core.Domain.Aggregate;

namespace Domain.Entities.Members.ValueObjects;

public class MemberFullName : ValueObject
{
    public string Value { get; }

    public MemberFullName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new MemberFullNameException();
        Value = value;
    }

    public static implicit operator string(MemberFullName fullName) => fullName.Value;
    public static implicit operator MemberFullName(string fullName) => new(fullName);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}