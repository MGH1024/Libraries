namespace Security.Application.Features.Auth.Commands.UserLogin;

public record UserLoginCommandResponse(
    string Token,
    DateTime TokenExpiry,
    string RefreshToken,
    DateTime RefreshTokenExpiry,
    bool IsSuccess);