using Application.Features.Users.Constants;
using MGH.Core.Application.Pipelines.Authorization;
using MGH.Core.Domain.Buses.Commands;

namespace Application.Features.Users.Commands.Create;

[Roles(UsersOperationClaims.AddUsers)]
public record CreateUserCommand(string FirstName, string LastName, string Email, string Password) : ICommand<CreatedUserResponse>;