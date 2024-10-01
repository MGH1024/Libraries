using Application.Features.Users.Constants;
using Application.Features.Users.Rules;
using AutoMapper;
using Domain;
using MGH.Core.Application.Pipelines.Authorization;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters.GetModels;

namespace Application.Features.Users.Commands.Delete;

[Roles(UsersOperationClaims.DeleteUsers)]
public class DeleteUserCommand : ICommand<DeletedUserResponse>
{
    public int Id { get; set; }
}

public class DeleteUserCommandHandler(IMapper mapper, IUow uow, UserBusinessRules userBusinessRules)
    : ICommandHandler<DeleteUserCommand, DeletedUserResponse>
{
    public async Task<DeletedUserResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var getUserModel = mapper.Map<GetModel<User>>(request, opt => opt.Items["CancellationToken"] = cancellationToken);
        var user = await uow.User.GetAsync(getUserModel);

        await userBusinessRules.UserShouldBeExistsWhenSelected(user);

        await uow.User.DeleteAsync(user!);
        await uow.CompleteAsync(cancellationToken);
        return mapper.Map<DeletedUserResponse>(user);
    }
}