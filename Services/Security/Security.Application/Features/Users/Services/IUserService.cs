using MGH.Core.Infrastructure.Persistence.Models.Filters.GetModels;
using MGH.Core.Infrastructure.Persistence.Models.Paging;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Security.Application.Features.Users.Services;

public interface IUserService
{
    Task<User> GetAsync(GetModel<User> getModel);
    Task<User> GetByEmailAsync(string email,CancellationToken cancellationToken);
    Task<IPaginate<User>> GetListAsync(GetListModelAsync<User> model);
    Task<User> AddAsync(User user,CancellationToken cancellationToken);
    Task<User> UpdateAsync(User user,CancellationToken cancellationToken);
    Task<User> DeleteAsync(User user, bool permanent = false);
}
