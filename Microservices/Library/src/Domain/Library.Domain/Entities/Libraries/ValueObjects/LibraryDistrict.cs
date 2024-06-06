using Domain.Entities.Libraries.Constant;
using Domain.Entities.Libraries.Exceptions;
using MGH.Core.Domain.Aggregate;

namespace Domain.Entities.Libraries.ValueObjects;

public class LibraryDistrict : ValueObject
{
    public District Value { get; }

    public LibraryDistrict(District value)
    {
        if ((int)value > 3 && (int)value <= 0)
            throw new LibraryDistrictException();
        Value = value;
    }

    public static implicit operator District(LibraryDistrict district) => district.Value;
    public static implicit operator LibraryDistrict(District district) => new(district);


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}