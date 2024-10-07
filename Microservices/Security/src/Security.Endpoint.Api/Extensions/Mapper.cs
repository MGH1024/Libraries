using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.RefreshToken;
using Application.Features.Auth.Commands.Register;
using Application.Features.Auth.Commands.RevokeToken;
using Application.Features.Users.Commands.UpdateFromAuth;
using Application.Features.Users.Queries.GetById;
using MGH.Core.Application.DTOs.Security;

namespace Api.Extensions;

public static class ApiMapper
{
    public static GetByIdUserQuery ToGetByIdUserQuery(this int userId)
    {
        return new GetByIdUserQuery
        {
            Id = userId
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

    public static void AddUserId(this UpdateUserFromAuthCommand updateUserFromAuthCommand, int userId)
    {
        updateUserFromAuthCommand.Id = userId;
    }
}