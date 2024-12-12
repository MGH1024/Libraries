namespace Library.Domain.Entities.Members.Exceptions;

public class MemberFullNameException : MemberException
{
    public MemberFullNameException() :base ("full name must not be empty")
    {
        
    }
}