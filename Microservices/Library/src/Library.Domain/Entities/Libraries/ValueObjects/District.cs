using Library.Domain.Entities.Libraries.Exceptions;
using MGH.Core.Domain.BaseEntity;

namespace Library.Domain.Entities.Libraries.ValueObjects;

public class District : ValueObject
{
    private Constant.DistrictEnum Value { get; }
    
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