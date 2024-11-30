using MGH.Core.Domain.Buses.Commands;

namespace Application.Features.Auth.Commands.Register;

public record RegisterCommand(RegisterCommandDto RegisterCommandDto) : ICommand<RegisteredResponse>;