using Domain.Entities.Libraries.Exceptions;
using MGH.Core.Domain.Aggregate;

namespace Domain.Entities.Libraries.ValueObjects;

public class LibraryCode : ValueObject
{
    public string Value { get; }

    public LibraryCode(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new LibraryCodeNullException();

        if (value.Length != 3)
            throw new LibraryCodeLengthException();

        Value = value;
    }

    public static implicit operator string(LibraryCode libraryCode) => libraryCode.Value;
    public static implicit operator LibraryCode(string libraryCode) => new(libraryCode);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}