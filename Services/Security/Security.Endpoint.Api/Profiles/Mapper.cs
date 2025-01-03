using Security.Application.Features.Auth.Commands.RevokeToken;
using Security.Application.Features.Users.Commands.UpdateFromAuth;
using Security.Application.Features.Users.Queries.GetById;

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