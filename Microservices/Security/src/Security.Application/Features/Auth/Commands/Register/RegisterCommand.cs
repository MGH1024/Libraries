using MGH.Core.Application.DTOs.Security;
using MGH.Core.Domain.Buses.Commands;

namespace Application.Features.Auth.Commands.Register;

public record RegisterCommand(UserForRegisterDto UserForRegisterDto) : ICommand<RegisteredResponse>;