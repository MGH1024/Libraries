using MGH.Core.Application.Rules;
using MGH.Core.CrossCutting.Exceptions.Types;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Infrastructure.Securities.Security.Hashing;
using Security.Application.Features.Auth.Constants;
using Security.Domain;

namespace Security.Application.Features.Auth.Rules;

public class AuthBusinessRules(IUow uow) : BaseBusinessRules, IAuthBusinessRules
{
    public Task UserShouldBeExistsWhenSelected(User user)
    {
        if (user == null)
            throw new BusinessException(AuthMessages.UserDoesNotExists);
        return Task.CompletedTask;
    }

    public Task RefreshTokenShouldBeExists(RefreshToken refreshToken)
    {
        if (refreshToken == null)
            throw new BusinessException(AuthMessages.RefreshDoesNotExists);
        return Task.CompletedTask;
    }

    public Task RefreshTokenShouldBeActive(RefreshToken refreshToken)
    {
        if (refreshToken.Revoked != null && DateTime.UtcNow >= refreshToken.Expires)
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