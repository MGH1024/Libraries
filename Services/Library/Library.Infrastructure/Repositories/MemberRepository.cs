using Library.Domain.Members;
using Library.Infrastructure.Contexts;
using MGH.Core.Infrastructure.Persistence.EF.Base.Repositories;

namespace Library.Infrastructure.Repositories;

public class MemberRepository(PublicLibraryDbContext libraryDbContext) 
    :Repository<Member,Guid>(libraryDbContext),IMemberRepository
{
    
}