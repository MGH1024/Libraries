namespace Security.Application.Features.Auth.Commands.RegisterUser;

public record RegisterUserCommandResponse(
    string Token,
    DateTime TokenExpiry,
    string RefreshToken,
    DateTime RefreshTokenExpiry);

