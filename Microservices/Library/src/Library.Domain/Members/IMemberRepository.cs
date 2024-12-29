using MGH.Core.Infrastructure.Persistence.Base;

namespace Library.Domain.Members;

public interface IMemberRepository : IRepository<Member, Guid>
{
    
}