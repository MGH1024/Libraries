using Domain.Entities.Libraries.Exceptions;

namespace Domain.Entities.Libraries.Guards;

public static class CodeGuard
{
    public static void CheckCodeIsNullOrEmpty(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new CodeNullException();
    }

    public static void CheckCodeLength(string code)
    {
      
        if (code.Length != 3)
            throw new CodeLengthException();
    }

    public static void CheckCodeIsNotString(string code)
    {
        if (!int.TryParse(code, out _))
            throw new CodeIsStringException();
    }
}