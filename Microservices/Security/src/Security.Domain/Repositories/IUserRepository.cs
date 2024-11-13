using MGH.Core.Infrastructure.Persistence.EF.Base.Repository;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Domain.Repositories;

public interface IUserRepository : IRepository<User, int>
{
    Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken);
}