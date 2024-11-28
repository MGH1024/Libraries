using Application.Features.Users.Constants;
using Application.Features.Users.Extensions;
using Application.Features.Users.Rules;
using AutoMapper;
using Domain;
using MGH.Core.Application.Pipelines.Authorization;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Securities.Security.Hashing;

namespace Application.Features.Users.Commands.Update;

[Roles(UsersOperationClaims.UpdateUsers)]
public class UpdateUserCommand(
    int id,
    string firstName,
    string lastName,
    string email,
    string password,
    string confirmPassword,
    string oldPassword)
    : ICommand<UpdatedUserResponse>
{
    public int Id { get; set; } = id;
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;
    public string ConfirmPassword { get; set; } = confirmPassword;
    public string OldPassword { get; set; } = oldPassword;


    public UpdateUserCommand() : this(0, string.Empty, string.Empty,
        string.Empty, string.Empty, string.Empty, string.Empty)
    {
    }
}

public class UpdateUserCommandHandler(
    IUow uow,
    IMapper mapper,
    UserBusinessRules userBusinessRules)
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