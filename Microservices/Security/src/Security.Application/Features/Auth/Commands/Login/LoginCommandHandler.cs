using Application.Features.Auth.Rules;
using Application.Services.AuthService;
using Domain.Repositories;
using MGH.Core.Domain.Buses.Commands;

namespace Application.Features.Auth.Commands.Login;

public class LoginCommandHandler(
    IUserRepository userRepository,
    IAuthService authService,
    AuthBusinessRules authBusinessRules) : ICommandHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.LoginCommandDto.Email, cancellationToken);
        await authBusinessRules.UserShouldBeExistsWhenSelected(user);
        await authBusinessRules.UserPasswordShouldBeMatch(user!.Id, request.LoginCommandDto.Password,cancellationToken);

        var createdAccessToken = await authService.CreateAccessTokenAsync(user, cancellationToken);
        var createdRefreshTkn = await authService.CreateRefreshToken(user, cancellationToken);
        var addedRefreshTkn = await authService.AddRefreshTokenAsync(createdRefreshTkn, cancellationToken);
        await authService.DeleteOldRefreshTokens(user.Id, cancellationToken);

        return new LoginResponse
        {
            Token = createdAccessToken.Token,
            RefreshToken = addedRefreshTkn.Token,
            IsSuccess = true,
            TokenExpiry = createdAccessToken.Expiration,
            RefreshTokenExpiry = addedRefreshTkn.Expires
        };
    }
}