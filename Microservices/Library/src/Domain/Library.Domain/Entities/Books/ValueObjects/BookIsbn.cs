using Domain.Entities.Books.Exceptions;
using MGH.Core.Domain.Concretes;

namespace Domain.Entities.Books.ValueObjects;

public class BookIsbn : ValueObject
{
    public string Value { get; }

    public BookIsbn(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new BookIsbnException();
        if (value.Length != 13)
            throw new IsbnInvalidLengthException();
        Value = value;
    }

    public static implicit operator string(BookIsbn bookIsbn) => bookIsbn.Value;
    public static implicit operator BookIsbn(string bookIsbn) => new(bookIsbn);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}