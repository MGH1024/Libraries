using MGH.Core.Application.Responses;
using MGH.Core.Infrastructure.Securities.Security.JWT;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Application.Features.Auth.Commands.RefreshToken;

public record RefreshTokenResponse(AccessToken AccessToken, RefreshTkn RefreshTkn) : IResponse;
