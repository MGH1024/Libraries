using Library.Domain.Entities.Libraries.Exceptions;
using MGH.Core.Domain.BaseEntity;

namespace Library.Domain.Entities.Libraries.ValueObjects;

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