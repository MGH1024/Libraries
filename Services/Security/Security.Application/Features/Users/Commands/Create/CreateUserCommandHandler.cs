using AutoMapper;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Infrastructure.Securities.Security.Hashing;
using Security.Application.Features.Users.Extensions;
using Security.Application.Features.Users.Rules;
using Security.Domain;

namespace Security.Application.Features.Users.Commands.Create;

public class CreateUserCommandHandler(IMapper mapper, IUserBusinessRules userBusinessRules, IUow uow)
    : ICommandHandler<CreateUserCommand, CreatedUserResponse>
{
    public async Task<CreatedUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        await userBusinessRules.UserEmailShouldNotExistsWhenInsert(request.Email, cancellationToken);
        var user = mapper.Map<User>(request);

        var hashingHelperModel = HashingHelper.CreatePasswordHash(request.Password);
        user.SetHashPassword(hashingHelperModel);

        var createdUser = await uow.User.AddAsync(user, cancellationToken);
        await uow.CompleteAsync(cancellationToken);

        return mapper.Map<CreatedUserResponse>(createdUser);
    }
}