using Application.Features.Users.Commands.Create;
using Application.Features.Users.Commands.Delete;
using Application.Features.Users.Commands.UpdateFromAuth;
using Application.Features.Users.Queries.GetById;
using Application.Features.Users.Queries.GetList;
using MediatR;
using MGH.Core.Application.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("{culture:CultureRouteConstraint}/api/[Controller]")]
public class UsersController(ISender sender) : AppController(sender)
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
        var getByIdUserQuery = new GetByIdUserQuery { Id = GetUserIdFromRequest() };
        var result = await Sender.Send(getByIdUserQuery, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest, CancellationToken cancellationToken)
    {
        var getListUserQuery = new GetListUserQuery { PageRequest = pageRequest };
        var result = await Sender.Send(getListUserQuery, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateUserCommand createUserCommand,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(createUserCommand, cancellationToken);
        return Created(uri: "", result);
    }

    [HttpPut("FromAuth")]
    public async Task<IActionResult> UpdateFromAuth([FromBody] UpdateUserFromAuthCommand updateUserFromAuthCommand,
        CancellationToken cancellationToken)
    {
        updateUserFromAuthCommand.Id = GetUserIdFromRequest();
        var result = await Sender.Send(updateUserFromAuthCommand, cancellationToken);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteUserCommand deleteUserCommand,
        CancellationToken cancellationToken)
    {
        DeletedUserResponse result = await Sender.Send(deleteUserCommand, cancellationToken);
        return Ok(result);
    }
}