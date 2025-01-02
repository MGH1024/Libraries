using Application.Features.Auth.Commands.RevokeToken;
using Application.Features.Users.Commands.UpdateFromAuth;
using Application.Features.Users.Queries.GetById;

namespace Security.Endpoint.Api.Profiles;

public static class ApiMapper
{
    public static GetUserByIdQuery ToGetByIdUserQuery(this int userId)
    {
        return new GetUserByIdQuery
        {
            Id = userId
        };
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