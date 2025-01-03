using Asp.Versioning;
using AutoMapper;
using MediatR;
using MGH.Core.Application.Requests;
using MGH.Core.Endpoint.Web;
using Microsoft.AspNetCore.Mvc;
using Security.Application.Features.Users.Commands.Create;
using Security.Application.Features.Users.Commands.Delete;
using Security.Application.Features.Users.Commands.Update;
using Security.Application.Features.Users.Commands.UpdateFromAuth;
using Security.Application.Features.Users.Queries.GetById;
using Security.Application.Features.Users.Queries.GetList;
using Security.Endpoint.Api.Profiles;

namespace Security.Endpoint.Api.Controllers.V1;

[ApiController] 
[ApiVersion(1)]
[Route("{culture:CultureRouteConstraint}/api/v{v:apiVersion}/[Controller]")]
public class UsersController(ISender sender, IMapper mapper) : AppController(sender)
{
    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById([FromRoute] GetUserByIdQuery getUserByIdQuery,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(getUserByIdQuery, cancellationToken);
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