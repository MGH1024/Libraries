using Domain.Entities.Members.Exceptions;
using MGH.Core.Domain.BaseEntity;

namespace Domain.Entities.Members.ValueObjects;

public class FullName : ValueObject
{
    public string Value { get; }

    public FullName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new MemberFullNameException();
        Value = value;
    }

    public static implicit operator string(FullName fullName) => fullName.Value;
    public static implicit operator FullName(string fullName) => new(fullName);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}