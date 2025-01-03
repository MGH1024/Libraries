namespace Security.Application.Features.Auth.Commands.RefreshToken;
public record RefreshTokenResponse(
    string Token,
    DateTime TokenExpiry,
    string RefreshToken,
    DateTime RefreshTokenExpiry);