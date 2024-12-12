namespace Library.Domain.Entities.Members.Exceptions;

public class MemberMobileNumberFormatException : MemberException
{
    public MemberMobileNumberFormatException() : base("member mobile has invalid format")
    {
    }
}