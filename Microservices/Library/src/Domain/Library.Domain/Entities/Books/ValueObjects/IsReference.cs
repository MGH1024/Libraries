
using MGH.Core.Domain.Aggregate;

namespace Domain.Entities.Books.ValueObjects;

public class BookIsReference :ValueObject
{
    public bool Value { get; }

    public BookIsReference(bool value)
    {
        Value = value;
    }
    
    public static implicit operator bool(BookIsReference bookIsReference) => bookIsReference.Value;
    public static implicit operator BookIsReference(bool bookIsReference) => new(bookIsReference);
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}