namespace Library.Domain.Entities.Members.Exceptions;

public class MemberIdException : MemberException
{
    public MemberIdException() : base("member Id should grater than zero")
    {
    }
}