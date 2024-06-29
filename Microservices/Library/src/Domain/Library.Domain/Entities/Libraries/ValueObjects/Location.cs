using Domain.Entities.Libraries.Exceptions;
using MGH.Core.Domain.Aggregate;

namespace Domain.Entities.Libraries.ValueObjects;

public class Location : ValueObject
{
    public string Value { get; }

    public Location(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new LibraryLocationException();
        Value = value;
    }

    public static implicit operator string(Location location) => location.Value;
    public static implicit operator Location(string location) => new(location);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}