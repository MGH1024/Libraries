using Domain;
using AutoMapper;
using MGH.Core.Domain.Buses.Commands;
using Application.Features.Users.Rules;
using Application.Services.AuthService;
using MGH.Core.Persistence.Models.Filters.GetModels;
using MGH.Core.Infrastructure.Securities.Security.Hashing;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Application.Features.Users.Commands.UpdateFromAuth;

public class UpdateUserFromAuthCommand(int id, string firstName, string lastName, string password)
    : ICommand<UpdatedUserFromAuthResponse>
{
    public int Id { get; set; } = id;
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public string Password { get; set; } = password;
    public string NewPassword { get; set; }

    public UpdateUserFromAuthCommand() : this(0, string.Empty, string.Empty, string.Empty)
    {
    }

    public class UpdateUserFromAuthCommandHandler(
        IUow uow,
        IMapper mapper,
        UserBusinessRules userBusinessRules,
        IAuthService authService)
        : ICommandHandler<UpdateUserFromAuthCommand, UpdatedUserFromAuthResponse>
    {
        public async Task<UpdatedUserFromAuthResponse> Handle(UpdateUserFromAuthCommand request,
            CancellationToken cancellationToken)
        {
            var user = await uow.User.GetAsync(new GetModel<User>
            {
                Predicate = u => u.Id == request.Id,
                CancellationToken = cancellationToken
            });

            await userBusinessRules.UserShouldBeExistsWhenSelected(user);
            await userBusinessRules.UserPasswordShouldBeMatched(user: user!, request.Password);
            await userBusinessRules.UserEmailShouldNotExistsWhenUpdate(user!.Id, user.Email);

            user = mapper.Map(request, user);
            if (request.NewPassword != null && !string.IsNullOrWhiteSpace(request.NewPassword))
            {
                HashingHelper.CreatePasswordHash(
                    request.Password,
                    passwordHash: out byte[] passwordHash,
                    passwordSalt: out byte[] passwordSalt
                );
                user!.PasswordHash = passwordHash;
                user!.PasswordSalt = passwordSalt;
            }

            var updatedUser = await uow.User.UpdateAsync(user!, cancellationToken);

            UpdatedUserFromAuthResponse response = mapper.Map<UpdatedUserFromAuthResponse>(updatedUser);
            response.AccessToken = await authService.CreateAccessToken(user!, cancellationToken);
            return response;
        }
    }
}