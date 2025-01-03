using AutoMapper;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Securities.Security.Hashing;
using Security.Application.Features.Users.Extensions;
using Security.Application.Features.Users.Rules;
using Security.Domain;

namespace Security.Application.Features.Users.Commands.Update;

public class UpdateUserCommandHandler(
    IUow uow,
    IMapper mapper,
    IUserBusinessRules userBusinessRules)
    : ICommandHandler<UpdateUserCommand, UpdatedUserResponse>
{
    public async Task<UpdatedUserResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await uow.User.GetAsync(request.Id,cancellationToken);
        await userBusinessRules.UserShouldBeExistsWhenSelected(user);
        await userBusinessRules.UserEmailShouldNotExistsWhenUpdate(user!.Id, user.Email,cancellationToken);

        user = mapper.Map(request, user);
        var hashingHelperModel = HashingHelper.CreatePasswordHash(request.Password);
        user.SetHashPassword(hashingHelperModel);

        await uow.User.UpdateAsync(user, cancellationToken);
        await uow.CompleteAsync(cancellationToken);
        return mapper.Map<UpdatedUserResponse>(user);
    }
}