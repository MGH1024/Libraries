using MGH.Core.Application.Responses;
using MGH.Core.Infrastructure.Securities.Security.JWT;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Application.Features.Auth.Commands.RefreshToken;
public record RefreshTokenResponse(
    string Token,
    DateTime TokenExpiry,
    string RefreshToken,
    DateTime RefreshTokenExpiry);