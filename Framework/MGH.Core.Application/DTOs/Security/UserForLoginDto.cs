using MGH.Core.Application.DTOs.Base;

namespace MGH.Core.Application.DTOs.Security;

public class UserForLoginDto(string email, string password) : IDto
{
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;

    public UserForLoginDto() : this(string.Empty, string.Empty)
    {
    }
}
