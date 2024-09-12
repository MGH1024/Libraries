using Application.Features.Auth.Constants;
using AutoMapper;
using Domain;
using MGH.Core.Application.Rules;
using MGH.Core.CrossCutting.Exceptions.Types;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Infrastructure.Securities.Security.Enums;
using MGH.Core.Infrastructure.Securities.Security.Hashing;
using MGH.Core.Persistence.Models.Filters.GetModels;

namespace Application.Features.Auth.Rules;

public class AuthBusinessRules(IUow uow, IMapper mapper) : BaseBusinessRules
{
    public Task EmailAuthenticatorShouldBeExists(EmailAuthenticator emailAuthenticator)
    {
        if (emailAuthenticator is null)
            throw new BusinessException(AuthMessages.EmailAuthenticatorDoesNotExists);
        return Task.CompletedTask;
    }

    public Task OtpAuthenticatorShouldBeExists(OtpAuthenticator otpAuthenticator)
    {
        if (otpAuthenticator is null)
            throw new BusinessException(AuthMessages.OtpAuthenticatorDoesNotExists);
        return Task.CompletedTask;
    }

    public Task OtpAuthenticatorThatVerifiedShouldNotBeExists(OtpAuthenticator otpAuthenticator)
    {
        if (otpAuthenticator is not null && otpAuthenticator.IsVerified)
            throw new BusinessException(AuthMessages.AlreadyVerifiedOtpAuthenticatorIsExists);
        return Task.CompletedTask;
    }

    public Task EmailAuthenticatorActivationKeyShouldBeExists(EmailAuthenticator emailAuthenticator)
    {
        if (emailAuthenticator.ActivationKey is null)
            throw new BusinessException(AuthMessages.EmailActivationKeyDoesNotExists);
        return Task.CompletedTask;
    }

    public Task UserShouldBeExistsWhenSelected(User user)
    {
        if (user == null)
            throw new BusinessException(AuthMessages.UserDoesNotExists);
        return Task.CompletedTask;
    }

    public Task UserShouldNotBeHaveAuthenticator(User user)
    {
        if (user.AuthenticatorType != AuthenticatorType.None)
            throw new BusinessException(AuthMessages.UserHaveAlreadyAAuthenticator);
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

    public async Task UserEmailShouldBeNotExists(string email,CancellationToken cancellationToken)
    {
        var baseGetUser = mapper.Map<GetBaseModel<User>>(email);
        var doesExists = await uow.User.AnyAsync(baseGetUser,cancellationToken);
        if (doesExists)
            throw new BusinessException(AuthMessages.UserMailAlreadyExists);
    }

    public async Task UserPasswordShouldBeMatch(int id, string password)
    {
        var userGetModel = mapper.Map<GetModel<User>>(id);
        var user = await uow.User.GetAsync(userGetModel);
        await UserShouldBeExistsWhenSelected(user);
        if (!HashingHelper.VerifyPasswordHash(password, user!.PasswordHash, user.PasswordSalt))
            throw new BusinessException(AuthMessages.PasswordDoesNotMatch);
    }
}