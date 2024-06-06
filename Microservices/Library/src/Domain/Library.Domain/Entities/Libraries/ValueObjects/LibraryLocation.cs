using Domain.Entities.Libraries.Exceptions;
using MGH.Core.Domain.Aggregate;

namespace Domain.Entities.Libraries.ValueObjects;

public class LibraryLocation : ValueObject
{
    public string Value { get; }

    public LibraryLocation(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new LibraryLocationException();
        Value = value;
    }

    public static implicit operator string(LibraryLocation libraryLocation) => libraryLocation.Value;
    public static implicit operator LibraryLocation(string location) => new(location);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}