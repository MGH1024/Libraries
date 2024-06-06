using Domain.Entities.Books.Exceptions;
using MGH.Core.Domain.Aggregate;

namespace Domain.Entities.Books.ValueObjects;

public class BookPublicationDate : ValueObject
{
    public DateTime Value { get; }

    public BookPublicationDate(DateTime value)
    {
        var now = DateTime.Now.Date;
        if (value > now)
            throw new BookPublicationDateException();
        Value = value;
    }

    public static implicit operator DateTime(BookPublicationDate publicationDate) => publicationDate.Value;
    public static implicit operator BookPublicationDate(DateTime publicationDate) => new(publicationDate);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}