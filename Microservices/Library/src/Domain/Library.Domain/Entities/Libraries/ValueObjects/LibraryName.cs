using Domain.Entities.Libraries.Exceptions;

using MGH.Core.Domain.Concretes;

namespace Domain.Entities.Libraries.ValueObjects;

public class LibraryName : ValueObject
{
    public string Value { get; }

    public LibraryName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new LibraryNameException();
        Value = value;
    }

    public static implicit operator string(LibraryName libraryName) => libraryName.Value;
    public static implicit operator LibraryName(string name) => new(name);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}