using Domain.Entities.Books.Exceptions;
using MGH.Core.Domain.Aggregate;

namespace Domain.Entities.Books.ValueObjects;

public class BookRegisterBorrowDate : ValueObject
{
    public DateTime Value { get; }

    public BookRegisterBorrowDate(DateTime value)
    {
        var now = DateTime.Now.Date;
        if (value.Date != now)
            throw new RegisterDateException();
        Value = value;
    }

    public static implicit operator DateTime(BookRegisterBorrowDate bookRegisterBorrowDate) => bookRegisterBorrowDate.Value;
    public static implicit operator BookRegisterBorrowDate(DateTime registerDate) => new(registerDate);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}