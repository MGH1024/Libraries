﻿using Application.Features.Users.Constants;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Application.Pipelines.Authorization;

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
}