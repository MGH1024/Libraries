using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Application.DTOs.Security;

namespace Application.Features.Auth.Commands.Login;

public record LoginCommand(LoginCommandDto LoginCommandDto) : ICommand<LoginResponse>;