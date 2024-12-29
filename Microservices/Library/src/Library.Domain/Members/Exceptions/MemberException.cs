using MGH.Core.CrossCutting.Exceptions.Types;

namespace Library.Domain.Members.Exceptions;

public class MemberException : BusinessException
{
    public MemberException(string message) : base(message)
    {
    }
}