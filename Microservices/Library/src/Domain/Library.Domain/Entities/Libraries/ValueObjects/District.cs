using Domain.Entities.Libraries.Constant;
using Domain.Entities.Libraries.Exceptions;
using MGH.Core.Domain.Aggregate;

namespace Domain.Entities.Libraries.ValueObjects;

public class District : ValueObject
{
    public Constant.District Value { get; }

    public District(Constant.District value)
    {
        if ((int)value > 3 && (int)value <= 0)
            throw new LibraryDistrictException();
        Value = value;
    }

    public static implicit operator Constant.District(District district) => district.Value;
    public static implicit operator District(Constant.District district) => new(district);


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}