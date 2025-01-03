using AutoMapper;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Persistence.Models.Filters.GetModels;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using Security.Application.Features.Users.Rules;
using Security.Domain;

namespace Security.Application.Features.Users.Commands.Delete;

public class DeleteUserCommandHandler(IMapper mapper, IUow uow, IUserBusinessRules userBusinessRules)
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