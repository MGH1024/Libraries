using Application.Features.Auth.Rules;
using Application.Services.AuthenticatorService;
using Application.Services.UsersService;
using Domain;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Infrastructure.Securities.Security.Enums;
using MGH.Core.Persistence.Models.Filters.GetModels;

namespace Application.Features.Auth.Commands.VerifyOtpAuthenticator;

public class VerifyOtpAuthenticatorCommand(int userId, string activationCode) : ICommand
{
    public int UserId { get; set; } = userId;
    public string ActivationCode { get; set; } = activationCode;

    public VerifyOtpAuthenticatorCommand() : this(0, string.Empty)
    {
    }
}

public class VerifyOtpAuthenticatorCommandHandler(
    IUow uow,
    AuthBusinessRules authBusinessRules,
    IUserService userService,
    IAuthenticatorService authenticatorService)
    : ICommandHandler<VerifyOtpAuthenticatorCommand>
{
    public async Task Handle(VerifyOtpAuthenticatorCommand request, CancellationToken cancellationToken)
    {
        var otpAuthenticator = await uow.OtpAuthenticator.GetAsync(new GetModel<OtpAuthenticator>
        {
            Predicate = e => e.UserId == request.UserId,
            CancellationToken = cancellationToken
        });
        await authBusinessRules.OtpAuthenticatorShouldBeExists(otpAuthenticator);
        var user = await userService.GetAsync(new GetModel<User>
        {
            Predicate = u => u.Id == request.UserId,
            CancellationToken = cancellationToken
        });
        await authBusinessRules.UserShouldBeExistsWhenSelected(user);
        otpAuthenticator!.IsVerified = true;
        user!.AuthenticatorType = AuthenticatorType.Otp;
        await authenticatorService.VerifyAuthenticatorCode(user, request.ActivationCode, cancellationToken);
        await uow.OtpAuthenticator.UpdateAsync(otpAuthenticator, cancellationToken);
        await userService.UpdateAsync(user, cancellationToken);
    }
}