using Application.Features.Users.Constants;
using MGH.Core.Application.Pipelines.Authorization;
using MGH.Core.Domain.Buses.Commands;

namespace Application.Features.Users.Commands.Update;

[Roles(UsersOperationClaims.UpdateUsers)]
public record UpdateUserCommand(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string ConfirmPassword,
    string OldPassword)
    : ICommand<UpdatedUserResponse>;