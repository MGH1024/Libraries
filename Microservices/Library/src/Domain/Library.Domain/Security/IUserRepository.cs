using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters;
using MGH.Core.Persistence.Models.Paging;

namespace Domain.Security;

public interface IUserRepository
{
    Task<User> GetAsync(GetBaseModel<User> getBaseModel);

    Task<IPaginate<User>> GetListAsync(GetListAsyncModel<User> getListAsyncModel);

    Task<IPaginate<User>> GetDynamicListAsync(
        GetDynamicListAsyncModel<User> dynamicListAsyncModel);

    Task<User> AddAsync(User entity, CancellationToken cancellationToken);
    Task<User> DeleteAsync(User entity, bool permanent = false);
}