namespace Application.Features.Auth.Commands.Login;

public record LoginResponse(
    string Token,
    DateTime TokenExpiry,
    string RefreshToken,
    DateTime RefreshTokenExpiry,
    bool IsSuccess);