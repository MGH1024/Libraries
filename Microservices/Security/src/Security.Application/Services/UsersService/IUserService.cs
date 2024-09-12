using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters.GetModels;
using MGH.Core.Persistence.Models.Paging;

namespace Application.Services.UsersService;

public interface IUserService
{
    Task<User> GetAsync(GetModel<User> getModel);
    Task<IPaginate<User>> GetListAsync(GetListModelAsync<User> model);
    Task<User> AddAsync(User user,CancellationToken cancellationToken);
    Task<User> UpdateAsync(User user,CancellationToken cancellationToken);
    Task<User> DeleteAsync(User user, bool permanent = false);
}
