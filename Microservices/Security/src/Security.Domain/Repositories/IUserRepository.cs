using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Base.Repository;

namespace Domain.Repositories;

public interface IUserRepository : IRepository<User, int>
{
}