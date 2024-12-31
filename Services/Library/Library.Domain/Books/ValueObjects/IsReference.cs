
using MGH.Core.Domain.BaseEntity;

namespace Library.Domain.Books.ValueObjects;

public class IsReference :ValueObject
{
    public bool Value { get; }

    public IsReference(bool value)
    {
        Value = value;
    }
    
    public static implicit operator bool(IsReference isReference) => isReference.Value;
    public static implicit operator IsReference(bool bookIsReference) => new(bookIsReference);
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}