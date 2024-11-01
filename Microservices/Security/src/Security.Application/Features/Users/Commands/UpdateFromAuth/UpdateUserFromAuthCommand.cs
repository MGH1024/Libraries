﻿using Application.Features.Users.Constants;
using Application.Features.Users.Extensions;
using Domain;
using AutoMapper;
using MGH.Core.Domain.Buses.Commands;
using Application.Features.Users.Rules;
using Application.Services.AuthService;
using MGH.Core.Application.Pipelines.Authorization;
using MGH.Core.Persistence.Models.Filters.GetModels;
using MGH.Core.Infrastructure.Securities.Security.Hashing;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Application.Features.Users.Commands.UpdateFromAuth;

[Roles(UsersOperationClaims.UpdateUsers)]
public class UpdateUserFromAuthCommand(
    int id,
    string firstName,
    string lastName,
    string password,
    string confirmPassword,
    string oldPassword)
    : ICommand<UpdatedUserFromAuthResponse>
{
    public int Id { get; set; } = id;
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public string Password { get; set; } = password;
    public string ConfirmPassword { get; set; } = confirmPassword;
    public string OldPassword { get; set; } = oldPassword;

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
            var user = await uow.User.GetAsync(request.Id,cancellationToken);

            await userBusinessRules.UserShouldBeExistsWhenSelected(user);
            await userBusinessRules.UserPasswordShouldBeMatched(user: user!, request.Password);
            await userBusinessRules.UserEmailShouldNotExistsWhenUpdate(user!.Id, user.Email,cancellationToken);

            user = mapper.Map(request, user);
            var hashingHelperModel = HashingHelper.CreatePasswordHash(request.Password);
            user.SetHashPassword(hashingHelperModel);

            var updatedUser = await uow.User.UpdateAsync(user!, cancellationToken);

            var response = mapper.Map<UpdatedUserFromAuthResponse>(updatedUser);
            response.AccessToken = await authService.CreateAccessTokenAsync(user!, cancellationToken);
            return response;
        }
    }
}