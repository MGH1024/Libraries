namespace Library.Domain.Entities.Members.Exceptions;

public class MemberNationalCodeLengthException : MemberException
{
    public MemberNationalCodeLengthException():base("length of national code not equal to 10")
    {
        
    }
}