using Application.Features.Users.Rules;
using AutoMapper;
using Domain;
using MediatR;
using MGH.Core.Application.Pipelines.Authorization;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Infrastructure.Securities.Security.Hashing;
using MGH.Core.Infrastructure.Securities.Security.Constants;

namespace Application.Features.Users.Commands.AddUser;

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

    public string[] Roles => new[]
        { GeneralOperationClaims.Admin, GeneralOperationClaims.Write, GeneralOperationClaims.Add };

    public class CreateUserCommandHandler(IMapper mapper, UserBusinessRules userBusinessRules, IUow uow)
        : IRequestHandler<CreateUserCommand, CreatedUserResponse>
    {
        public async Task<CreatedUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            await userBusinessRules.UserEmailShouldNotExistsWhenInsert(request.Email);
            var user = mapper.Map<User>(request);

            HashingHelper.CreatePasswordHash(
                request.Password,
                passwordHash: out byte[] passwordHash,
                passwordSalt: out byte[] passwordSalt
            );
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            var createdUser = await uow.User.AddAsync(user, cancellationToken);
            await uow.CompleteAsync(cancellationToken);
            var response = mapper.Map<CreatedUserResponse>(createdUser);
            return response;
        }
    }
}