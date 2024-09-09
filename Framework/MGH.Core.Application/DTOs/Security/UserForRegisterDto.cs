using MGH.Core.Application.DTOs.Base;

namespace MGH.Core.Application.DTOs.Security;

public class UserForRegisterDto(string email, string password, string firstName, string lastName) : IDto
{
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;

    public UserForRegisterDto() : this(string.Empty, string.Empty, string.Empty, string.Empty)
    {
    }
}