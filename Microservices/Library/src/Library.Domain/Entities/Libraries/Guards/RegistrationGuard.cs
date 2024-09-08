using Domain.Entities.Libraries.Exceptions;

namespace Domain.Entities.Libraries.Guards;

public static class RegistrationGuard
{
    public static void CheckRegistrationDateValue(DateTime registrationDate)
    {
        var now = DateTime.Now.Date;
        if (registrationDate.Date >= now || registrationDate < now.AddYears(-100))
            throw new RegistrationDateException();
    }
}