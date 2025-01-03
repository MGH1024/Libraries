using MGH.Core.Domain.Buses.Commands;

namespace Security.Application.Features.Auth.Commands.RegisterUser;

public record RegisterUserCommand(RegisterUserCommandDto RegisterUserCommandDto)
    : ICommand<RegisterUserCommandResponse>;