using MGH.Core.Application.DTOs.Base;

namespace Application.Features.Users.Queries.GetList;

public class GetListUserListItemDto(int id, string firstName, string lastName, string email, bool status)
    : IDto
{
    public int Id { get; set; } = id;
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public string Email { get; set; } = email;
    public bool Status { get; set; } = status;

    public GetListUserListItemDto() : this(0, string.Empty, string.Empty, string.Empty, false)
    {
    }
}
