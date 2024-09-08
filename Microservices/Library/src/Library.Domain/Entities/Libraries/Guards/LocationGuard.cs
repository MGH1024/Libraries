using Domain.Entities.Libraries.Exceptions;

namespace Domain.Entities.Libraries.Guards;

public static class LocationGuard
{
    public static void CheckLocationIsNullOrEmpty(string location)
    {
        if (string.IsNullOrWhiteSpace(location))
            throw new LocationNullException();
    }
}