using Library.Domain.Libraries.Exceptions;
using MGH.Core.Domain.BaseModels;

namespace Library.Domain.Libraries.ValueObjects;

public class Name : ValueObject
{
    public string Value { get; }
    
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