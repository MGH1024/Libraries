using Application.Features.Auth.Rules;
using Application.Services.AuthenticatorService;
using Application.Services.UsersService;
using AutoMapper;
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
    IMapper mapper,
    IAuthenticatorService authenticatorService)
    : ICommandHandler<VerifyOtpAuthenticatorCommand>
{
    public async Task Handle(VerifyOtpAuthenticatorCommand request, CancellationToken cancellationToken)
    {
        var getOtpAuthenticatorModel = mapper.Map<GetModel<OtpAuthenticator>>(request, opt => opt.Items["CancellationToken"] = cancellationToken);
        var otpAuthenticator = await uow.OtpAuthenticator.GetAsync(getOtpAuthenticatorModel);

        await authBusinessRules.OtpAuthenticatorShouldBeExists(otpAuthenticator);
        var getUserModel = mapper.Map<GetModel<User>>(request, opt => opt.Items["CancellationToken"] = cancellationToken);

        var user = await userService.GetAsync(getUserModel);
        await authBusinessRules.UserShouldBeExistsWhenSelected(user);

        otpAuthenticator!.IsVerified = true;
        user!.AuthenticatorType = AuthenticatorType.Otp;
        await authenticatorService.VerifyAuthenticatorCode(user, request.ActivationCode, cancellationToken);
        await uow.OtpAuthenticator.UpdateAsync(otpAuthenticator, cancellationToken);
        await userService.UpdateAsync(user, cancellationToken);
    }
}