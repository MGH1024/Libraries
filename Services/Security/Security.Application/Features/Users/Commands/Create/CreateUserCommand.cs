using MGH.Core.Application.Pipelines.Authorization;
using MGH.Core.Domain.Buses.Commands;
using Security.Application.Features.Users.Constants;

namespace Security.Application.Features.Users.Commands.Create;

[Roles(UsersOperationClaims.AddUsers)]
public record CreateUserCommand(string FirstName, string LastName, string Email, string Password) : ICommand<CreatedUserResponse>;