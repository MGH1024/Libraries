using MGH.Core.Domain.Buses.Commands;

namespace Application.Features.Auth.Commands.RegisterUser;

public record RegisterUserCommand(RegisterUserCommandDto RegisterUserCommandDto)
    : ICommand<RegisterUserCommandResponse>;