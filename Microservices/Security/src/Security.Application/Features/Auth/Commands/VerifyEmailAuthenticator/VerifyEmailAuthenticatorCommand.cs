using Application.Features.Auth.Rules;
using AutoMapper;
using Domain;
using MGH.Core.Application.DTOs.Security;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters.GetModels;

namespace Application.Features.Auth.Commands.VerifyEmailAuthenticator;

public class VerifyEmailAuthenticatorCommand(VerifyEmailAuthenticatorDto verifyEmailAuthenticatorDto) : ICommand
{
    public VerifyEmailAuthenticatorDto VerifyEmailAuthenticatorDto { get; set; } = verifyEmailAuthenticatorDto;

    public VerifyEmailAuthenticatorCommand() : this(null!)
    {
    }
}

public class VerifyEmailAuthenticatorCommandHandler(
    IUow uow,
    IMapper mapper,
    AuthBusinessRules authBusinessRules)
    : ICommandHandler<VerifyEmailAuthenticatorCommand>
{
    public async Task Handle(VerifyEmailAuthenticatorCommand request, CancellationToken cancellationToken)
    {
        var getUserModel = mapper.Map<GetModel<EmailAuthenticator>>(request, opt =>
            opt.Items["CancellationToken"] = cancellationToken);
        var emailAuthenticator = await uow.EmailAuthenticator.GetAsync(getUserModel);

        await authBusinessRules.EmailAuthenticatorShouldBeExists(emailAuthenticator);
        await authBusinessRules.EmailAuthenticatorActivationKeyShouldBeExists(emailAuthenticator!);

        emailAuthenticator!.ActivationKey = null;
        emailAuthenticator.IsVerified = true;
        await uow.EmailAuthenticator.UpdateAsync(emailAuthenticator, cancellationToken);
    }
}