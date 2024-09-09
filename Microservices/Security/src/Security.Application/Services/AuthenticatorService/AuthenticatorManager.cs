using Domain;
using MGH.Core.CrossCutting.Exceptions.Types;
using MGH.Core.Infrastructure.Mail.Base;
using MGH.Core.Infrastructure.Mail.MailKitImplementations.Models;
using MGH.Core.Infrastructure.Securities.Security.EmailAuthenticator;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Infrastructure.Securities.Security.Enums;
using MGH.Core.Infrastructure.Securities.Security.OtpAuthenticator;
using MGH.Core.Persistence.Models.Filters.GetModels;
using MimeKit;

namespace Application.Services.AuthenticatorService;

public class AuthenticatorManager(
    IUow uow,
    IEmailAuthenticatorHelper emailAuthenticatorHelper,
    IMailService mailService,
    IOtpAuthenticatorHelper otpAuthenticatorHelper) : IAuthenticatorService
{
    private readonly IMailService _mailService = mailService;

    public async Task<EmailAuthenticator> CreateEmailAuthenticator(User user, CancellationToken cancellationToken)
    {
        EmailAuthenticator emailAuthenticator =
            new()
            {
                UserId = user.Id,
                ActivationKey = await emailAuthenticatorHelper.CreateEmailActivationKey(),
                IsVerified = false
            };
        return emailAuthenticator;
    }

    public async Task<OtpAuthenticator> CreateOtpAuthenticator(User user, CancellationToken cancellationToken)
    {
        OtpAuthenticator otpAuthenticator =
            new()
            {
                UserId = user.Id,
                SecretKey = await otpAuthenticatorHelper.GenerateSecretKey(),
                IsVerified = false
            };
        return otpAuthenticator;
    }

    public async Task<string> ConvertSecretKeyToString(byte[] secretKey)
    {
        string result = await otpAuthenticatorHelper.ConvertSecretKeyToString(secretKey);
        return result;
    }

    public async Task SendAuthenticatorCode(User user, CancellationToken cancellationToken)
    {
        if (user.AuthenticatorType is AuthenticatorType.Email)
            await SendAuthenticatorCodeWithEmail(user, cancellationToken);
    }

    public async Task VerifyAuthenticatorCode(User user, string authenticatorCode, CancellationToken cancellationToken)
    {
        if (user.AuthenticatorType is AuthenticatorType.Email)
            await VerifyAuthenticatorCodeWithEmail(user, authenticatorCode, cancellationToken);
        else if (user.AuthenticatorType is AuthenticatorType.Otp)
            await VerifyAuthenticatorCodeWithOtp(user, authenticatorCode);
    }

    private async Task SendAuthenticatorCodeWithEmail(User user, CancellationToken cancellationToken)
    {
        EmailAuthenticator emailAuthenticator = await uow.EmailAuthenticator
            .GetAsync(new GetModel<EmailAuthenticator> { Predicate = e => e.UserId == user.Id });
        if (emailAuthenticator is null)
            throw new NotFoundException("Email Authenticator not found.");
        if (!emailAuthenticator.IsVerified)
            throw new BusinessException("Email Authenticator must be is verified.");

        string authenticatorCode = await emailAuthenticatorHelper.CreateEmailActivationCode();
        emailAuthenticator.ActivationKey = authenticatorCode;
        await uow.EmailAuthenticator.UpdateAsync(emailAuthenticator, cancellationToken);

        var toEmailList = new List<MailboxAddress> { new(name: $"{user.FirstName} {user.LastName}", user.Email) };

        _mailService.SendMail(
            new Mail
            {
                ToList = toEmailList,
                Subject = "Authenticator Code - MGH",
                TextBody = $"Enter your authenticator code: {authenticatorCode}"
            }
        );
    }

    private async Task VerifyAuthenticatorCodeWithEmail(User user, string authenticatorCode,
        CancellationToken cancellationToken)
    {
        var emailAuthenticator = await uow.EmailAuthenticator
            .GetAsync(new GetModel<EmailAuthenticator>
                { Predicate = e => e.UserId == user.Id });
        if (emailAuthenticator is null)
            throw new NotFoundException("Email Authenticator not found.");
        if (emailAuthenticator.ActivationKey != authenticatorCode)
            throw new BusinessException("Authenticator code is invalid.");
        emailAuthenticator.ActivationKey = null;
        await uow.EmailAuthenticator.UpdateAsync(emailAuthenticator, cancellationToken);
    }

    private async Task VerifyAuthenticatorCodeWithOtp(User user, string authenticatorCode)
    {
        var otpAuthenticator =
            await uow.OtpAuthenticator.GetAsync(
                new GetModel<OtpAuthenticator>
                    { Predicate = e => e.UserId == user.Id });
        if (otpAuthenticator is null)
            throw new NotFoundException("Otp Authenticator not found.");
        bool result = await otpAuthenticatorHelper.VerifyCode(otpAuthenticator.SecretKey, authenticatorCode);
        if (!result)
            throw new BusinessException("Authenticator code is invalid.");
    }
}