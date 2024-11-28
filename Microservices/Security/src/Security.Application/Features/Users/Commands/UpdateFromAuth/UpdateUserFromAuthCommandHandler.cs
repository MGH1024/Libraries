using Application.Features.Auth.Services;
using Application.Features.Users.Extensions;
using Application.Features.Users.Rules;
using AutoMapper;
using Domain;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Securities.Security.Hashing;

namespace Application.Features.Users.Commands.UpdateFromAuth;

public class UpdateUserFromAuthCommandHandler(
    IUow uow,
    IMapper mapper,
    IUserBusinessRules userBusinessRules,
    IAuthService authService)
    : ICommandHandler<UpdateUserFromAuthCommand, UpdatedUserFromAuthResponse>
{
    public async Task<UpdatedUserFromAuthResponse> Handle(UpdateUserFromAuthCommand request,
        CancellationToken cancellationToken)
    {
        var user = await uow.User.GetAsync(request.Id,cancellationToken);

        await userBusinessRules.UserShouldBeExistsWhenSelected(user);
        await userBusinessRules.UserPasswordShouldBeMatched(user: user!, request.Password);
        await userBusinessRules.UserEmailShouldNotExistsWhenUpdate(user!.Id, user.Email,cancellationToken);

        user = mapper.Map(request, user);
        var hashingHelperModel = HashingHelper.CreatePasswordHash(request.Password);
        user.SetHashPassword(hashingHelperModel);

        var updatedUser = await uow.User.UpdateAsync(user!, cancellationToken);

        var response = mapper.Map<UpdatedUserFromAuthResponse>(updatedUser);
        response.AccessToken = await authService.CreateAccessTokenAsync(user!, cancellationToken);
        return response;
    }
}