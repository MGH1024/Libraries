using Domain.Entities.Books.Exceptions;
using MGH.Core.Domain.Aggregate;

namespace Domain.Entities.Books.ValueObjects;

public class BookUniqueCode : ValueObject
{
    public string Value { get; }

    public BookUniqueCode(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new BookUniqueCodeException();

        Value = value;
    }

    public static implicit operator string(BookUniqueCode bookUniqueCode) => bookUniqueCode.Value;
    public static implicit operator BookUniqueCode(string bookUniqueCode) => new(bookUniqueCode);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}