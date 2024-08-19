using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters.GetModels;
using MGH.Core.Persistence.Models.Paging;


namespace Domain.Security;

public interface IUserRepository
{
    Task<User> GetAsync(GetModel<User> getModel);

    Task<IPaginate<User>> GetListAsync(GetListAsyncModel<User> getListAsyncModel);

    Task<IPaginate<User>> GetDynamicListAsync(GetDynamicListAsyncModel<User> dynamicListAsyncModel);

    Task<User> AddAsync(User entity, CancellationToken cancellationToken);
    Task<User> DeleteAsync(User entity, bool permanent = false);
    Task<bool> AnyAsync(Base<User> @base);
    Task<User> UpdateAsync(User entity,CancellationToken  cancellationToken);
}