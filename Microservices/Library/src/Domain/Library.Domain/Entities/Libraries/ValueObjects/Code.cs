using Domain.Entities.Libraries.Exceptions;
using MGH.Core.Domain.Aggregate;

namespace Domain.Entities.Libraries.ValueObjects;

public class Code : ValueObject
{
    public string Value { get; }

    public Code(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new LibraryCodeNullException();

        if (value.Length != 3)
            throw new LibraryCodeLengthException();

        Value = value;
    }

    public static implicit operator string(Code code) => code.Value;
    public static implicit operator Code(string libraryCode) => new(libraryCode);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}