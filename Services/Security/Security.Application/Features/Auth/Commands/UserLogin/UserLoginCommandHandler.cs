using Application.Features.Auth.Rules;
using Application.Features.Auth.Services;
using Domain;
using MGH.Core.Domain.Buses.Commands;

namespace Application.Features.Auth.Commands.UserLogin;

public class UserLoginCommandHandler(
    IUow uow,
    IAuthService authService,
    IAuthBusinessRules authBusinessRules) : ICommandHandler<UserLoginCommand, UserLoginCommandResponse>
{
    public async Task<UserLoginCommandResponse> Handle(UserLoginCommand request, CancellationToken cancellationToken)
    {
        var user = await uow.User.GetByEmailAsync(request.UserLoginCommandDto.Email, cancellationToken);
        await authBusinessRules.UserShouldBeExistsWhenSelected(user);
        await authBusinessRules.UserPasswordShouldBeMatch(user!.Id, request.UserLoginCommandDto.Password,
            cancellationToken);

        var createdAccessToken = await authService.CreateAccessTokenAsync(user, cancellationToken);
        var createdRefreshTkn = await authService.CreateRefreshToken(user);
        var addedRefreshTkn = await authService.AddRefreshTokenAsync(createdRefreshTkn, cancellationToken);
        await authService.DeleteOldRefreshTokens(user.Id, cancellationToken);

        return new UserLoginCommandResponse(createdAccessToken.Token, createdAccessToken.Expiration, addedRefreshTkn.Token,
            addedRefreshTkn.Expires, true);
    }
}