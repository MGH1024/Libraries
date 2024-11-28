using MGH.Core.Application.DTOs.Base;

namespace Application.Features.Auth.Commands.Login;

public record LoginCommandDto(string Email, string Password) : IDto;
