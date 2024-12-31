using Library.Domain.Books.Exceptions;
using MGH.Core.Domain.BaseEntity;

namespace Library.Domain.Books.ValueObjects;

public class UniqueCode : ValueObject
{
    public string Value { get; }

    public UniqueCode(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new BookUniqueCodeException();

        Value = value;
    }

    public static implicit operator string(UniqueCode uniqueCode) => uniqueCode.Value;
    public static implicit operator UniqueCode(string bookUniqueCode) => new(bookUniqueCode);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}