using MGH.Core.Application.Pipelines.Authorization;
using MGH.Core.Domain.Buses.Commands;
using Security.Application.Features.Users.Constants;

namespace Security.Application.Features.Users.Commands.Delete;

[Roles(UsersOperationClaims.DeleteUsers)]
public record DeleteUserCommand(int Id) : ICommand<DeletedUserResponse>;