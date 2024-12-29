using Library.Domain.Books.Exceptions;
using MGH.Core.Domain.BaseEntity;

namespace Library.Domain.Books.ValueObjects;

public class Title : ValueObject
{
    public string Value { get; }

    public Title(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new BookTitleException();
        Value = value;
    }

    public static implicit operator string(Title title) => title.Value;
    public static implicit operator Title(string bookTitle) => new(bookTitle);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}