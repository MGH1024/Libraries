using Domain.Entities.Books.Exceptions;

using MGH.Core.Domain.Concretes;

namespace Domain.Entities.Books.ValueObjects;

public class BookReturnDate: ValueObject
{
    public DateTime Value { get; }

    public BookReturnDate(DateTime value)
    {
        var now = DateTime.Now.Date;
        if (value <= now.Date)
            throw new DueDateException();
        Value = value;
    }
    
    public static implicit operator DateTime(BookReturnDate bookReturnDate) => bookReturnDate.Value;
    public static implicit operator BookReturnDate(DateTime dueDate) => new(dueDate);
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}