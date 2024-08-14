using Application.Features.Users.Rules;
using AutoMapper;
using Domain;
using MGH.Core.Application.Pipelines.Authorization;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Securities.Security.Constants;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters.GetModels;

namespace Application.Features.Users.Commands.Delete;

public class DeleteUserCommand : ICommand<DeletedUserResponse>, ISecuredRequest
{
    public int Id { get; set; }

    public string[] Roles => new[]
    {
        GeneralOperationClaims.Admin, GeneralOperationClaims.Write,
        GeneralOperationClaims.Delete
    };

    public class DeleteUserCommandHandler(
        IMapper mapper,
        IUow uow,
        UserBusinessRules userBusinessRules)
        : ICommandHandler<DeleteUserCommand, DeletedUserResponse>
    {
        public async Task<DeletedUserResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await uow.User.GetAsync(new GetModel<User>
            {
                Predicate = u => u.Id == request.Id,
                CancellationToken = cancellationToken
            });
            await userBusinessRules.UserShouldBeExistsWhenSelected(user);

            await uow.User.DeleteAsync(user!);
            await uow.CompleteAsync(cancellationToken);
            var response = mapper.Map<DeletedUserResponse>(user);
            return response;
        }
    }
}