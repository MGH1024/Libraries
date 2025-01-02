using Library.Domain.Books.Exceptions;
using MGH.Core.Domain.BaseModels;

namespace Library.Domain.Books.ValueObjects;

public class Isbn : ValueObject
{
    public string Value { get; }

    public Isbn(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new BookIsbnException();
        if (value.Length != 13)
            throw new IsbnInvalidLengthException();
        Value = value;
    }

    public static implicit operator string(Isbn isbn) => isbn.Value;
    public static implicit operator Isbn(string bookIsbn) => new(bookIsbn);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}