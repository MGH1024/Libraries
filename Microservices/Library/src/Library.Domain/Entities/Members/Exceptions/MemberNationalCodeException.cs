namespace Library.Domain.Entities.Members.Exceptions;

public class MemberNationalCodeNullException : MemberException
{
    public MemberNationalCodeNullException() : base("national code name must not be empty")
    {
    }
}