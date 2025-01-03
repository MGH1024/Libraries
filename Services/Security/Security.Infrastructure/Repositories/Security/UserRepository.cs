using MGH.Core.Infrastructure.Persistence.EF.Base.Repository;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Security.Domain.Repositories;
using Security.Infrastructure.Contexts;

namespace Security.Infrastructure.Repositories.Security;

public class UserRepository(SecurityDbContext securityDbContext) : Repository<User, int>(securityDbContext), 
    IUserRepository
{
    private IQueryable<User> Query() => securityDbContext.Set<User>();

    public async Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await Query().Where(a => a.Email == email).FirstOrDefaultAsync(cancellationToken);
    }
}