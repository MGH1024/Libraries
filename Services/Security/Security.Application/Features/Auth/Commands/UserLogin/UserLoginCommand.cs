using MGH.Core.Domain.Buses.Commands;

namespace Application.Features.Auth.Commands.UserLogin;

public record UserLoginCommand(UserLoginCommandDto UserLoginCommandDto) : ICommand<UserLoginCommandResponse>;