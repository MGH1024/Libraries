using Application.Features.Auth.Rules;
using Application.Services.AuthenticatorService;
using Application.Services.UsersService;
using AutoMapper;
using Domain;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters.GetModels;

namespace Application.Features.Auth.Commands.EnableOtpAuthenticator;

public class EnableOtpAuthenticatorCommand : ICommand<EnabledOtpAuthenticatorResponse>
{
    public int UserId { get; set; }
}

public class EnableOtpAuthenticatorCommandHandler(
    IUserService userService,
    IUow uow,
    IMapper mapper,
    AuthBusinessRules authBusinessRules,
    IAuthenticatorService authenticatorService)
    : ICommandHandler<EnableOtpAuthenticatorCommand, EnabledOtpAuthenticatorResponse>
{
    public async Task<EnabledOtpAuthenticatorResponse> Handle(EnableOtpAuthenticatorCommand request, CancellationToken cancellationToken)
    {
        var getUserModel = mapper.Map<GetModel<User>>(request, opt =>
            opt.Items["CancellationToken"] = cancellationToken);
        var user = await userService.GetAsync(getUserModel);
        
        await authBusinessRules.UserShouldBeExistsWhenSelected(user);
        await authBusinessRules.UserShouldNotBeHaveAuthenticator(user!);

        var getUserModelOtpAuthenticator = mapper.Map<GetModel<OtpAuthenticator>>(request, opt =>
            opt.Items["CancellationToken"] = cancellationToken);
        var doesExistOtpAuthenticator = await uow.OtpAuthenticator.GetAsync(getUserModelOtpAuthenticator);
        
        await authBusinessRules.OtpAuthenticatorThatVerifiedShouldNotBeExists(doesExistOtpAuthenticator);
        if (doesExistOtpAuthenticator is not null)
            await uow.OtpAuthenticator.DeleteAsync(doesExistOtpAuthenticator,false,cancellationToken);

        var newOtpAuthenticator = await authenticatorService.CreateOtpAuthenticator(user!, cancellationToken);
        var addedOtpAuthenticator = await uow.OtpAuthenticator.AddAsync(newOtpAuthenticator, cancellationToken);
        return mapper.Map<EnabledOtpAuthenticatorResponse>(addedOtpAuthenticator);
    }
}