using Domain;
using MGH.Core.Domain.Buses.Commands;
using Application.Features.Auth.Rules;
using Application.Features.Auth.Services;

namespace Application.Features.Auth.Commands.Login;

public class LoginCommandHandler(
    IUow uow,
    IAuthService authService,
    IAuthBusinessRules authBusinessRules) : ICommandHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await uow.User.GetByEmailAsync(request.LoginCommandDto.Email, cancellationToken);
        await authBusinessRules.UserShouldBeExistsWhenSelected(user);
        await authBusinessRules.UserPasswordShouldBeMatch(user!.Id, request.LoginCommandDto.Password,
            cancellationToken);

        var createdAccessToken = await authService.CreateAccessTokenAsync(user, cancellationToken);
        var createdRefreshTkn = await authService.CreateRefreshToken(user);
        var addedRefreshTkn = await authService.AddRefreshTokenAsync(createdRefreshTkn, cancellationToken);
        await authService.DeleteOldRefreshTokens(user.Id, cancellationToken);

        return new LoginResponse(createdAccessToken.Token, createdAccessToken.Expiration, addedRefreshTkn.Token,
            addedRefreshTkn.Expires, true);
    }
}