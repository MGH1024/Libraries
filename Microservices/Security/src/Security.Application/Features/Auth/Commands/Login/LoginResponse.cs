namespace Application.Features.Auth.Commands.Login;

public class LoginResponse
{
    public string Token { get; set; }
    public DateTime TokenExpiry { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiry { get; set; }
    public bool IsSuccess { get; set; }
}