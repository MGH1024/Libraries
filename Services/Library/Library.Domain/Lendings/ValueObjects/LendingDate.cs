using Library.Domain.Lendings.Exceptions;
using MGH.Core.Domain.Base;

namespace Library.Domain.Lendings.ValueObjects;

public class LendingDate : ValueObject
{
    public DateTime Value { get; }

    public LendingDate(DateTime value)
    {
        if (Value < DateTime.Today)
            throw new InvalidLendingDateException();
        Value = value;
    }

    public static implicit operator DateTime(LendingDate lendingDate) => lendingDate.Value;
    public static implicit operator LendingDate(DateTime reservationDate) => new(reservationDate);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}