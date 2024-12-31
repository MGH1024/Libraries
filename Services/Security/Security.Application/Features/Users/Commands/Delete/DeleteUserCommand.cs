using MGH.Core.Domain.Buses.Commands;
using Application.Features.Users.Constants;
using MGH.Core.Application.Pipelines.Authorization;

namespace Application.Features.Users.Commands.Delete;

[Roles(UsersOperationClaims.DeleteUsers)]
public record DeleteUserCommand(int Id) : ICommand<DeletedUserResponse>;