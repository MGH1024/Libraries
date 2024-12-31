namespace Library.Domain.Members.Exceptions;

public class MemberMobileNumberNotNumberException :MemberException
{
    public MemberMobileNumberNotNumberException():base("member mobile should be number")
    {
            
    }
}