using Domain.Entities.Libraries.Exceptions;

namespace Domain.Entities.Libraries.Guards;

public static class NameGuard
{
    public static void CheckNameIsNullOrEmpty(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new NameException();
    }
}