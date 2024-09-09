using Application.Features.Auth.Rules;
using Domain;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters.GetModels;

namespace Application.Features.Auth.Commands.VerifyEmailAuthenticator;

public class VerifyEmailAuthenticatorCommand(string activationKey) : ICommand
{
    public string ActivationKey { get; set; } = activationKey;

    public VerifyEmailAuthenticatorCommand() : this(string.Empty)
    {
    }
}

public class VerifyEmailAuthenticatorCommandHandler(
    IUow uow,
    AuthBusinessRules authBusinessRules)
    : ICommandHandler<VerifyEmailAuthenticatorCommand>
{
    public async Task Handle(VerifyEmailAuthenticatorCommand request, CancellationToken cancellationToken)
    {
        var emailAuthenticator = await uow.EmailAuthenticator.GetAsync(new GetModel<EmailAuthenticator>
            {
                Predicate = e => e.ActivationKey == request.ActivationKey,
                CancellationToken = cancellationToken
            }
        );
        await authBusinessRules.EmailAuthenticatorShouldBeExists(emailAuthenticator);
        await authBusinessRules.EmailAuthenticatorActivationKeyShouldBeExists(emailAuthenticator!);

        emailAuthenticator!.ActivationKey = null;
        emailAuthenticator.IsVerified = true;
        await uow.EmailAuthenticator.UpdateAsync(emailAuthenticator, cancellationToken);
    }
}