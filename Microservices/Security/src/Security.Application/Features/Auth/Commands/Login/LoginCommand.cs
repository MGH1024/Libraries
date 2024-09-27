using AutoMapper;
using Domain.Repositories;
using MGH.Core.Domain.Buses.Commands;
using Application.Features.Auth.Rules;
using Application.Services.AuthService;
using MGH.Core.Application.DTOs.Security;
using Application.Services.AuthenticatorService;
using MGH.Core.Infrastructure.Securities.Security.Enums;

namespace Application.Features.Auth.Commands.Login;

public class LoginCommand(UserForLoginDto userForLoginDto, string ipAddress) : ICommand<LoginResponse>
{
    public UserForLoginDto UserForLoginDto { get; set; } = userForLoginDto;
    public string IpAddress { get; set; } = ipAddress;

    public LoginCommand() : this(null!, string.Empty)
    {
    }
}

public class LoginCommandHandler(
    IUserRepository userRepository,
    IAuthService authService,
    AuthBusinessRules authBusinessRules,
    IAuthenticatorService authenticatorService) : ICommandHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.UserForLoginDto.Email, cancellationToken);
        await authBusinessRules.UserShouldBeExistsWhenSelected(user);
        await authBusinessRules.UserPasswordShouldBeMatch(user!.Id, request.UserForLoginDto.Password);

        var loggedResponse = new LoginResponse();

        if (user.AuthenticatorType is not AuthenticatorType.None)
        {
            if (request.UserForLoginDto.AuthenticatorCode is null)
            {
                await authenticatorService.SendAuthenticatorCode(user, cancellationToken);
                loggedResponse.RequiredAuthenticatorType = user.AuthenticatorType;
                return loggedResponse;
            }

            await authenticatorService.VerifyAuthenticatorCode(user, request.UserForLoginDto.AuthenticatorCode,
                cancellationToken);
        }

        var createdAccessToken = await authService.CreateAccessTokenAsync(user, cancellationToken);
        var createdRefreshTkn = await authService.CreateRefreshToken(user, request.IpAddress, cancellationToken);
        var addedRefreshTkn = await authService.AddRefreshTokenAsync(createdRefreshTkn, cancellationToken);
        await authService.DeleteOldRefreshTokens(user.Id, cancellationToken);

        loggedResponse.AccessToken = createdAccessToken;
        loggedResponse.RefreshTkn = addedRefreshTkn;
        loggedResponse.IsSuccess = true;
        return loggedResponse;
    }
}