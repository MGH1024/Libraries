namespace Library.Domain.Members.Exceptions;

public class MemberMobileNumberLengthException :MemberException
{
    public MemberMobileNumberLengthException() :base("member mobile number must has 11 character")
    {
            
    }
}