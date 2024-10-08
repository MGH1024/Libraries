using Application.Features.Auth.Constants;
using Domain;
using MGH.Core.Application.Rules;
using MGH.Core.CrossCutting.Exceptions.Types;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Infrastructure.Securities.Security.Hashing;

namespace Application.Features.Auth.Rules;

public class AuthBusinessRules(IUow uow) : BaseBusinessRules
{
    public Task UserShouldBeExistsWhenSelected(User user)
    {
        if (user == null)
            throw new BusinessException(AuthMessages.UserDoesNotExists);
        return Task.CompletedTask;
    }

    public Task RefreshTokenShouldBeExists(RefreshTkn refreshTkn)
    {
        if (refreshTkn == null)
            throw new BusinessException(AuthMessages.RefreshDoesNotExists);
        return Task.CompletedTask;
    }

    public Task RefreshTokenShouldBeActive(RefreshTkn refreshTkn)
    {
        if (refreshTkn.Revoked != null && DateTime.UtcNow >= refreshTkn.Expires)
            throw new BusinessException(AuthMessages.InvalidRefreshToken);
        return Task.CompletedTask;
    }

    public async Task UserEmailShouldBeNotExists(string email, CancellationToken cancellationToken)
    {
        var user = await uow.User.GetByEmailAsync(email, cancellationToken);
        if (user is not null)
            throw new BusinessException(AuthMessages.UserMailAlreadyExists);
    }

    public async Task UserPasswordShouldBeMatch(int id, string password,CancellationToken cancellationToken)
    {
        var user = await uow.User.GetAsync(id,cancellationToken);
        await UserShouldBeExistsWhenSelected(user);
        if (!HashingHelper.VerifyPasswordHash(password, user!.PasswordHash, user.PasswordSalt))
            throw new BusinessException(AuthMessages.PasswordDoesNotMatch);
    }
}