using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.RefreshToken;
using Application.Features.Auth.Commands.Register;
using Application.Features.Auth.Commands.RevokeToken;
using MGH.Core.Application.DTOs.Security;

namespace Api.Extensions;

public static class ApiMapper
{
    public static LoginCommand ToLoginCommand(this UserForLoginDto userForLoginDto, string ipAddress)
    {
        return new LoginCommand
        {
            UserForLoginDto = userForLoginDto,
            IpAddress = ipAddress
        };
    }

    public static RegisterCommand ToRegisterCommand(this UserForRegisterDto userForRegisterDto, string ipAddress)
    {
        return new RegisterCommand
        {
            UserForRegisterDto = userForRegisterDto,
            IpAddress = ipAddress
        };
    }

    public static RefreshTokenCommand ToRefreshTokenCommand(string refreshToken, string ipAddress)
    {
        return new RefreshTokenCommand
        {
            RefreshToken = refreshToken,
            IpAddress = ipAddress
        };
    }


    public static RevokeTokenCommand ToRevokeTokenCommand(this string refreshToken, string ipAddress)
    {
        return new RevokeTokenCommand
        {
            Token = refreshToken,
            IpAddress = ipAddress
        };
    }
    
}