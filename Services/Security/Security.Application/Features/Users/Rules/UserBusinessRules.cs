using MGH.Core.Application.Rules;
using MGH.Core.CrossCutting.Exceptions.Types;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Infrastructure.Securities.Security.Hashing;
using Security.Application.Features.Auth.Constants;
using Security.Application.Features.Users.Extensions;
using Security.Domain;

namespace Security.Application.Features.Users.Rules;

public class UserBusinessRules(IUow uow) : BaseBusinessRules,IUserBusinessRules
{
    public Task UserShouldBeExistsWhenSelected(User user)
    {
        if (user is null)
            throw new BusinessException(AuthMessages.UserDoesNotExists);
        return Task.CompletedTask;
    }

    public async Task UserIdShouldBeExistsWhenSelected(int id,CancellationToken cancellationToken)
    {
        var doesExist = await uow.User.AnyAsync(id.ToGetBaseUser(),cancellationToken);
        if (doesExist)
            throw new BusinessException(AuthMessages.UserDoesNotExists);
    }

    public Task UserPasswordShouldBeMatched(User user, string password)
    {
        if (!HashingHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            throw new BusinessException(AuthMessages.PasswordDoesNotMatch);
        return Task.CompletedTask;
    }

    public async Task UserEmailShouldNotExistsWhenInsert(string email,CancellationToken cancellationToken)
    {
        var doesExists = await uow.User.AnyAsync(email.ToGetBaseUser(), cancellationToken);
        if (doesExists)
            throw new BusinessException(AuthMessages.UserMailAlreadyExists);
    }

    public async Task UserEmailShouldNotExistsWhenUpdate(int id, string email,CancellationToken cancellationToken)
    {
        var doesExists = await uow.User.AnyAsync(email.ToGetBaseUser(id),cancellationToken);
        if (doesExists)
            throw new BusinessException(AuthMessages.UserMailAlreadyExists);
    }
}