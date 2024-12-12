using Library.Domain.Entities.Books.Exceptions;
using MGH.Core.Domain.BaseEntity;

namespace Library.Domain.Entities.Books.ValueObjects;

public class RegisterBorrowDate : ValueObject
{
    public DateTime Value { get; }

    public RegisterBorrowDate(DateTime value)
    {
        var now = DateTime.Now.Date;
        if (value.Date != now)
            throw new RegisterDateException();
        Value = value;
    }

    public static implicit operator DateTime(RegisterBorrowDate registerBorrowDate) => registerBorrowDate.Value;
    public static implicit operator RegisterBorrowDate(DateTime registerDate) => new(registerDate);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}