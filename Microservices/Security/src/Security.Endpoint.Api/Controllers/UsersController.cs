using MediatR;
using AutoMapper;
using Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using MGH.Core.Application.Requests;
using Application.Features.Users.Commands.Create;
using Application.Features.Users.Commands.Delete;
using Application.Features.Users.Commands.Update;
using Application.Features.Users.Queries.GetById;
using Application.Features.Users.Queries.GetList;
using Application.Features.Users.Commands.UpdateFromAuth;

namespace Api.Controllers;

[ApiController]
[Route("{culture:CultureRouteConstraint}/api/[Controller]")]
public class UsersController(ISender sender, IMapper mapper) : AppController(sender)
{
    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById([FromRoute] GetByIdUserQuery getByIdUserQuery,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(getByIdUserQuery, cancellationToken);
        return Ok(result);
    }

    [HttpGet("GetFromAuth")]
    public async Task<IActionResult> GetFromAuth(CancellationToken cancellationToken)
    {
        var getByIdUserQuery = GetUserIdFromRequest().ToGetByIdUserQuery();
        var result = await Sender.Send(getByIdUserQuery, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest, CancellationToken cancellationToken)
    {
        var getListUserQuery = mapper.Map<GetListUserQuery>(pageRequest);
        var result = await Sender.Send(getListUserQuery, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateUserCommand createUserCommand, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(createUserCommand, cancellationToken);
        return Created(uri: "", result);
    }


    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateUserCommand updateUserCommand, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(updateUserCommand, cancellationToken);
        return Ok(result);
    }

    [HttpPut("FromAuth")]
    public async Task<IActionResult> UpdateFromAuth([FromBody] UpdateUserFromAuthCommand updateUserFromAuthCommand,
        CancellationToken cancellationToken)
    {
        updateUserFromAuthCommand.AddUserId(GetUserIdFromRequest());
        var result = await Sender.Send(updateUserFromAuthCommand, cancellationToken);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteUserCommand deleteUserCommand, CancellationToken cancellationToken)
    {
        DeletedUserResponse result = await Sender.Send(deleteUserCommand, cancellationToken);
        return Ok(result);
    }
}