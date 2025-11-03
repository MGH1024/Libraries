using Library.Domain.Libraries.Exceptions;
using MGH.Core.Domain.Base;

namespace Library.Domain.Libraries.ValueObjects;

public class District : ValueObject
{
    public Constant.DistrictEnum Value { get; }
    
    public District(Constant.DistrictEnum value)
    {
        if ((int)value > 3 && (int)value <= 0)
            throw new DistrictException();
        Value = value;
    }

    public static implicit operator Constant.DistrictEnum (District district) => district.Value;
    public static implicit operator District(Constant.DistrictEnum code) => new(code);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}