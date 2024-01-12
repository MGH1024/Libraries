namespace Domain.Entities.Members.Exceptions;

public class MemberMobileNumberNullException : MemberException
{
    public MemberMobileNumberNullException() : base("member mobile number must not be empty")
    {
    }
}