using Application.Features.Auth.Rules;
using Application.Services.AuthenticatorService;
using Application.Services.AuthService;
using Application.Services.UsersService;
using AutoMapper;
using MGH.Core.Application.DTOs.Security;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Infrastructure.Securities.Security.Enums;
using MGH.Core.Persistence.Models.Filters.GetModels;

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
    IUserService userService,
    IAuthService authService,
    AuthBusinessRules authBusinessRules,
    IAuthenticatorService authenticatorService,
    IMapper mapper)
    : ICommandHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var getUserModel = mapper.Map<GetModel<User>>(request, opt =>
            opt.Items["CancellationToken"] = cancellationToken);
        var user = await userService.GetAsync(getUserModel);

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

        var createdAccessToken = await authService.CreateAccessToken(user, cancellationToken);

        var createdRefreshTkn = await authService.CreateRefreshToken(user, request.IpAddress, cancellationToken);
        var addedRefreshTkn = await authService.AddRefreshToken(createdRefreshTkn, cancellationToken);
        await authService.DeleteOldRefreshTokens(user.Id, cancellationToken);

        loggedResponse.AccessToken = createdAccessToken;
        loggedResponse.RefreshTkn = addedRefreshTkn;
        return loggedResponse;
    }
}