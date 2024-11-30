using MGH.Core.Domain.Buses.Commands;

namespace Application.Features.Auth.Commands.Login;

public record LoginCommand(LoginCommandDto LoginCommandDto) : ICommand<LoginResponse>;