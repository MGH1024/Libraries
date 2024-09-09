using System.Web;
using Application.Features.Auth.Commands.EnableEmailAuthenticator;
using Application.Features.Auth.Commands.Register;
using MGH.Core.Infrastructure.Mail.MailKitImplementations.Models;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Infrastructure.Securities.Security.Hashing;
using MimeKit;
using Nest;

namespace Application.Features.Auth.Extensions;

public static class AuthExtension
{
    public static User ToUser(this RegisterCommand registerCommand, HashingHelperModel hashingHelperModel)
    {
        return new User
        {
            Email = registerCommand.UserForRegisterDto.Email,
            FirstName = registerCommand.UserForRegisterDto.FirstName,
            LastName = registerCommand.UserForRegisterDto.LastName,
            PasswordHash = hashingHelperModel.PasswordHash,
            PasswordSalt = hashingHelperModel.PasswordSalt
        };
    }

    public static List<MailboxAddress> ToMailboxAddress(this User user)
    {
        return new List<MailboxAddress> { new(name: $"{user.FirstName} {user.LastName}", user.Email) };
    }

    public static Mail ToMail(this List<MailboxAddress> mailboxAddresses, EnableEmailAuthenticatorCommand request,
        EmailAuthenticator emailAuthenticator)
    {
        return new Mail
        {
            ToList = mailboxAddresses,
            Subject = "Verify Your Email - MGH",
            TextBody =
                $"Click on the link to verify your email:" +
                $" {request.VerifyEmailUrlPrefix}?ActivationKey={HttpUtility.UrlEncode(emailAuthenticator.ActivationKey)}"
        };
    }
}