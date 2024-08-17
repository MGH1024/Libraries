using Application.Features.Users.Extensions;
using Application.Features.Users.Rules;
using AutoMapper;
using Domain;
using MGH.Core.Application.Pipelines.Authorization;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Securities.Security.Constants;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Infrastructure.Securities.Security.Hashing;

namespace Application.Features.Users.Commands.Create;

public class CreateUserCommand(string firstName, string lastName, string email, string password)
    : ICommand<CreatedUserResponse>, ISecuredRequest
{
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;

    public CreateUserCommand() : this(string.Empty, string.Empty, string.Empty, string.Empty)
    {
    }

    public string[] Roles => new[] {GeneralOperationClaims.AddUsers };

    public class CreateUserCommandHandler(IMapper mapper, UserBusinessRules userBusinessRules, IUow uow)
        : ICommandHandler<CreateUserCommand, CreatedUserResponse>
    {
        public async Task<CreatedUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            await userBusinessRules.UserEmailShouldNotExistsWhenInsert(request.Email);
            var user = mapper.Map<User>(request);

            var hashingHelperModel = HashingHelper.CreatePasswordHash(request.Password);
            user.SetHashPassword(hashingHelperModel);
            
            var createdUser = await uow.User.AddAsync(user, cancellationToken);
            await uow.CompleteAsync(cancellationToken);
            
            return mapper.Map<CreatedUserResponse>(createdUser);
        }
    }
}