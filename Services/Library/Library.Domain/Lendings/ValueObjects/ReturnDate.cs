using Library.Domain.Lendings.Exceptions;
using MGH.Core.Domain.BaseModels;

namespace Library.Domain.Lendings.ValueObjects;

public class ReturnDate: ValueObject
{
    public DateTime Value { get; }

    public ReturnDate(DateTime value)
    {
        var now = DateTime.Now.Date;
        if (value <= now.Date)
            throw new DueDateException();
        Value = value;
    }
    
    public static implicit operator DateTime(ReturnDate returnDate) => returnDate.Value;
    public static implicit operator ReturnDate(DateTime dueDate) => new(dueDate);
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}