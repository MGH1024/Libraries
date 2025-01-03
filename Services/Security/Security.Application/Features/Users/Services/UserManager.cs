using MGH.Core.Infrastructure.Persistence.Models.Filters.GetModels;
using MGH.Core.Infrastructure.Persistence.Models.Paging;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using Security.Application.Features.Users.Rules;
using Security.Domain;

namespace Security.Application.Features.Users.Services;

public class UserManager(IUow uow, IUserBusinessRules userBusinessRules) : IUserService
{
    public async Task<User> GetAsync(GetModel<User> getModel)
    {
        return await uow.User.GetAsync(getModel);
    }

    public async Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await uow.User.GetByEmailAsync(email, cancellationToken);
    }

    public async Task<IPaginate<User>> GetListAsync(GetListModelAsync<User> model)
    {
        return await uow.User.GetListAsync(model);
    }

    public async Task<User> AddAsync(User user, CancellationToken cancellationToken)
    {
        await userBusinessRules.UserEmailShouldNotExistsWhenInsert(user.Email, cancellationToken);
        return await uow.User.AddAsync(user, cancellationToken);
    }

    public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken)
    {
        await userBusinessRules.UserEmailShouldNotExistsWhenUpdate(user.Id, user.Email, cancellationToken);
        return await uow.User.UpdateAsync(user, cancellationToken);
    }

    public async Task<User> DeleteAsync(User user, bool permanent = false)
    {
        return await uow.User.DeleteAsync(user);
    }
}