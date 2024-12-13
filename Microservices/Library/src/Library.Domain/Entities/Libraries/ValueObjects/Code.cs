using Library.Domain.Entities.Libraries.Exceptions;
using MGH.Core.Domain.BaseEntity;

namespace Library.Domain.Entities.Libraries.ValueObjects;

public class Code : ValueObject
{
    public string Value { get; }
    
    public Code(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new CodeNullException();
        Value = value;
    }

    public static implicit operator string(Code code) => code.Value;
    public static implicit operator Code(string code) => new(code);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}