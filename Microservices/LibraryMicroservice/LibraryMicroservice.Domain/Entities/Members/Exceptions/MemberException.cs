using MGH.Core.CrossCutting.Exceptions.Types;

namespace Domain.Entities.Members.Exceptions;

public class MemberException : BusinessException
{
    public MemberException(string message) : base(message)
    {
    }
}