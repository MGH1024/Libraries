using Domain.Entities.Libraries.Exceptions;
using MGH.Core.Domain.BaseEntity;

namespace Domain.Entities.Libraries.ValueObjects;

public class Location : ValueObject
{
    private string Value { get; }
    
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