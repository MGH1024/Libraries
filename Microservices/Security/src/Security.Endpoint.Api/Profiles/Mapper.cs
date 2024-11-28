using Application.Features.Auth.Commands.RefreshToken;
using Application.Features.Auth.Commands.Register;
using Application.Features.Auth.Commands.RevokeToken;
using Application.Features.Users.Commands.UpdateFromAuth;
using Application.Features.Users.Queries.GetById;
using MGH.Core.Application.DTOs.Security;

namespace Api.Profiles;

public static class ApiMapper
{
    public static GetUserByIdQuery ToGetByIdUserQuery(this int userId)
    {
        return new GetUserByIdQuery
        {
            Id = userId
        };
    }
    
    public static RegisterCommand ToRegisterCommand(this UserForRegisterDto userForRegisterDto, string ipAddress)
    {
        return new RegisterCommand(userForRegisterDto);
    }

    public static RefreshTokenCommand ToRefreshTokenCommand(string refreshToken)
    {
        return new RefreshTokenCommand(refreshToken);
    }

    public static RevokeTokenCommand ToRevokeTokenCommand(this string refreshToken)
    {
        return new RevokeTokenCommand(refreshToken);
    }

    public static void AddUserId(this UpdateUserFromAuthCommand updateUserFromAuthCommand, int userId)
    {
        updateUserFromAuthCommand.Id = userId;
    }
}