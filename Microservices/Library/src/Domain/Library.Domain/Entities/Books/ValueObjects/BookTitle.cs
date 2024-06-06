using Domain.Entities.Books.Exceptions;
using MGH.Core.Domain.Aggregate;

namespace Domain.Entities.Books.ValueObjects;

public class BookTitle : ValueObject
{
    public string Value { get; }

    public BookTitle(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new BookTitleException();
        Value = value;
    }

    public static implicit operator string(BookTitle bookTitle) => bookTitle.Value;
    public static implicit operator BookTitle(string bookTitle) => new(bookTitle);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}