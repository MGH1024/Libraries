using Application.Features.Auth.Rules;
using Application.Services.AuthenticatorService;
using Application.Services.UsersService;
using Domain;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters.GetModels;

namespace Application.Features.Auth.Commands.EnableOtpAuthenticator;

public class EnableOtpAuthenticatorCommand : ICommand<EnabledOtpAuthenticatorResponse>
{
    public int UserId { get; set; }

    public class EnableOtpAuthenticatorCommandHandler(
        IUserService userService,
        IUow  uow,
        AuthBusinessRules authBusinessRules,
        IAuthenticatorService authenticatorService)
        : ICommandHandler<EnableOtpAuthenticatorCommand, EnabledOtpAuthenticatorResponse>
    {
        public async Task<EnabledOtpAuthenticatorResponse> Handle(
            EnableOtpAuthenticatorCommand request,
            CancellationToken cancellationToken
        )
        {
            var user = await userService.GetAsync(new GetModel<User>()
                { Predicate = u => u.Id == request.UserId, CancellationToken = cancellationToken });
            await authBusinessRules.UserShouldBeExistsWhenSelected(user);
            await authBusinessRules.UserShouldNotBeHaveAuthenticator(user!);

            var doesExistOtpAuthenticator = await uow.OtpAuthenticator.GetAsync(new GetModel<OtpAuthenticator>
            {
                Predicate = o => o.UserId == request.UserId,
                CancellationToken = cancellationToken
            });
            await authBusinessRules.OtpAuthenticatorThatVerifiedShouldNotBeExists(doesExistOtpAuthenticator);
            if (doesExistOtpAuthenticator is not null)
                await uow.OtpAuthenticator.DeleteAsync(doesExistOtpAuthenticator);

            OtpAuthenticator newOtpAuthenticator =
                await authenticatorService.CreateOtpAuthenticator(user!, cancellationToken);
            OtpAuthenticator addedOtpAuthenticator =
                await uow.OtpAuthenticator.AddAsync(newOtpAuthenticator, cancellationToken);

            EnabledOtpAuthenticatorResponse enabledOtpAuthenticatorDto =
                new()
                {
                    SecretKey = await authenticatorService.ConvertSecretKeyToString(addedOtpAuthenticator.SecretKey)
                };
            return enabledOtpAuthenticatorDto;
        }
    }
}