using MGH.Core.Application.Responses;

namespace Application.Features.Users.Commands.Update;

public class UpdatedUserResponse(int id, string firstName, string lastName, string email, bool status)
    : IResponse
{
    public int Id { get; set; } = id;
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public string Email { get; set; } = email;
    public bool Status { get; set; } = status;

    public UpdatedUserResponse() : this(0, string.Empty, string.Empty, string.Empty, false)
    {
    }
}
