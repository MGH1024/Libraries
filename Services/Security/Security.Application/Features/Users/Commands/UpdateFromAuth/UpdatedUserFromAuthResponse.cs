using MGH.Core.Application.Responses;
using MGH.Core.Infrastructure.Securities.Security.JWT;

namespace Security.Application.Features.Users.Commands.UpdateFromAuth;

public class UpdatedUserFromAuthResponse(
    int id,
    string firstName,
    string lastName,
    string email,
    AccessToken accessToken)
    : IResponse
{
    public int Id { get; set; } = id;
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public string Email { get; set; } = email;
    public AccessToken AccessToken { get; set; } = accessToken;

    public UpdatedUserFromAuthResponse() : this(0, string.Empty, string.Empty, string.Empty, null!)
    {
    }
}
