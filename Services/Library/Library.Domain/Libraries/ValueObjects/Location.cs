using Library.Domain.Libraries.Exceptions;
using MGH.Core.Domain.Base;

namespace Library.Domain.Libraries.ValueObjects;

public class Location : ValueObject
{
    public string Value { get; }
    
    public Location(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new LocationNullException();
        Value = value;
    }

    public static implicit operator string(Location code) => code.Value;
    public static implicit operator Location(string code) => new(code);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}