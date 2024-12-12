using MGH.Core.Domain.BaseEntity;
using Domain.Entities.Libraries.Exceptions;

namespace Domain.Entities.Libraries.ValueObjects;

public class Name : ValueObject
{
    private string Value { get; }
    
    public Name(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new NameException();
        Value = value;
    }

    public static implicit operator string(Name name) => name.Value;
    public static implicit operator Name(string name) => new(name);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}