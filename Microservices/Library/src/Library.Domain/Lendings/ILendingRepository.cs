using MGH.Core.Infrastructure.Persistence.Base;

namespace Library.Domain.Lendings;

public interface ILendingRepository :IAggregateRepository<Lending, Guid>
{
    
}