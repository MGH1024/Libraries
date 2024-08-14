using Application.Features.Users.Rules;
using Domain.Security;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters.GetModels;
using MGH.Core.Persistence.Models.Paging;

namespace Application.Services.UsersService;

public class UserManager(IUserRepository userRepository, UserBusinessRules userBusinessRules)
    : IUserService
{
    public async Task<User> GetAsync( GetModel<User> getModel)
    {
        var user = await userRepository.GetAsync(getModel);
        return user;
    }

    public async Task<IPaginate<User>> GetListAsync( GetListAsyncModel<User> model)
    {
        var userList = await userRepository.GetListAsync(model);
        return userList;
    }

    public async Task<User> AddAsync(User user, CancellationToken cancellationToken)
    {
        await userBusinessRules.UserEmailShouldNotExistsWhenInsert(user.Email);
        var addedUser = await userRepository.AddAsync(user, cancellationToken);
        return addedUser;
    }

    public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken)
    {
        await userBusinessRules.UserEmailShouldNotExistsWhenUpdate(user.Id, user.Email);
        var updatedUser = await userRepository.UpdateAsync(user, cancellationToken);
        return updatedUser;
    }

    public async Task<User> DeleteAsync(User user, bool permanent = false)
    {
        var deletedUser = await userRepository.DeleteAsync(user);
        return deletedUser;
    }
}