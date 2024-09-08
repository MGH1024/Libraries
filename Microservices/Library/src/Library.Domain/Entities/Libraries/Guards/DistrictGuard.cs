using Domain.Entities.Libraries.Exceptions;

namespace Domain.Entities.Libraries.Guards;

public static class DistrictGuard
{
    public static void CheckDistrictValue(Constant.District value)
    {
        if ((int)value > 3 && (int)value <= 0)
            throw new DistrictException();
    }
}