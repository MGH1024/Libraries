using Application.Features.Auth.Rules;
using Application.Services.AuthenticatorService;
using Application.Services.UsersService;
using MimeKit;
using System.Web;
using Domain;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Mail.Base;
using MGH.Core.Infrastructure.Mail.MailKitImplementations.Models;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Infrastructure.Securities.Security.Enums;
using MGH.Core.Persistence.Models.Filters.GetModels;

namespace Application.Features.Auth.Commands.EnableEmailAuthenticator;

public class EnableEmailAuthenticatorCommand(int userId, string verifyEmailUrlPrefix) : ICommand
{
    public int UserId { get; set; } = userId;
    public string VerifyEmailUrlPrefix { get; set; } = verifyEmailUrlPrefix;

    public EnableEmailAuthenticatorCommand() : this(0, string.Empty)
    {
    }

    public class EnableEmailAuthenticatorCommandHandler(
        IUserService userService,
        IUow uow,
        IMailService mailService,
        AuthBusinessRules authBusinessRules,
        IAuthenticatorService authenticatorService)
        : ICommandHandler<EnableEmailAuthenticatorCommand>
    {
        public async Task Handle(EnableEmailAuthenticatorCommand request, CancellationToken cancellationToken)
        {
            var user = await userService.GetAsync(new GetModel<User>()
                { Predicate = u => u.Id == request.UserId, CancellationToken = cancellationToken });
            await authBusinessRules.UserShouldBeExistsWhenSelected(user);
            await authBusinessRules.UserShouldNotBeHaveAuthenticator(user!);

            user!.AuthenticatorType = AuthenticatorType.Email;
            await userService.UpdateAsync(user, cancellationToken);

            EmailAuthenticator emailAuthenticator =
                await authenticatorService.CreateEmailAuthenticator(user, cancellationToken);
            EmailAuthenticator addedEmailAuthenticator =
                await uow.EmailAuthenticator.AddAsync(emailAuthenticator, cancellationToken);

            var toEmailList = new List<MailboxAddress> { new(name: $"{user.FirstName} {user.LastName}", user.Email) };

            mailService.SendMail(
                new Mail
                {
                    ToList = toEmailList,
                    Subject = "Verify Your Email - NArchitecture",
                    TextBody =
                        $"Click on the link to verify your email: {request.VerifyEmailUrlPrefix}?ActivationKey={HttpUtility.UrlEncode(addedEmailAuthenticator.ActivationKey)}"
                }
            );
        }
    }
}