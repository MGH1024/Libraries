using Library.Domain.Lendings;
using Library.Infrastructure.Contexts;
using MGH.Core.Infrastructure.Persistence.EF.Base.Repositories;

namespace Library.Infrastructure.Repositories;

public class LendingRepository(LibraryDbContext libraryDbContext) :Repository<Lending,Guid>,ILendingRepository
{
    
}