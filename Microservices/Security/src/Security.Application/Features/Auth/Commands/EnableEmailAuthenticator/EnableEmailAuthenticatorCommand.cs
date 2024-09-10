using Application.Features.Auth.Rules;
using Application.Services.AuthenticatorService;
using Application.Services.UsersService;
using Application.Features.Auth.Extensions;
using AutoMapper;
using Domain;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Mail.Base;
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
}

public class EnableEmailAuthenticatorCommandHandler(
    IUserService userService,
    IUow uow,
    IMailService mailService,
    IMapper mapper,
    AuthBusinessRules authBusinessRules,
    IAuthenticatorService authenticatorService)
    : ICommandHandler<EnableEmailAuthenticatorCommand>
{
    public async Task Handle(EnableEmailAuthenticatorCommand request, CancellationToken cancellationToken)
    {
        var getUserModel = mapper.Map<GetModel<User>>(request, opt =>
            opt.Items["CancellationToken"] = cancellationToken);
        var user = await userService.GetAsync(getUserModel);
        
        await authBusinessRules.UserShouldBeExistsWhenSelected(user);
        await authBusinessRules.UserShouldNotBeHaveAuthenticator(user!);
        user!.AuthenticatorType = AuthenticatorType.Email;
        await userService.UpdateAsync(user, cancellationToken);

        var emailAuthenticator = await authenticatorService.CreateEmailAuthenticator(user, cancellationToken);
        var addedEmailAuthenticator = await uow.EmailAuthenticator.AddAsync(emailAuthenticator, cancellationToken);

        var toEmailList = user.ToMailboxAddress();
        mailService.SendMail(toEmailList.ToMail(request, addedEmailAuthenticator));
    }
}