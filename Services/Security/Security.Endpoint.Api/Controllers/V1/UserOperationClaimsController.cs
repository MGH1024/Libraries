using Asp.Versioning;
using MediatR;
using MGH.Core.Application.Requests;
using MGH.Core.Application.Responses;
using MGH.Core.Endpoint.Web;
using Microsoft.AspNetCore.Mvc;
using Security.Application.Features.UserOperationClaims.Commands.Create;
using Security.Application.Features.UserOperationClaims.Commands.Delete;
using Security.Application.Features.UserOperationClaims.Commands.Update;
using Security.Application.Features.UserOperationClaims.Queries.GetById;
using Security.Application.Features.UserOperationClaims.Queries.GetList;

namespace Security.Endpoint.Api.Controllers.V1;

[ApiController]
[ApiVersion(1)]
[Route("{culture:CultureRouteConstraint}/api/v{v:apiVersion}/[Controller]")]
public class UserOperationClaimsController(ISender sender) : AppController(sender)
{
    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById([FromRoute] GetByIdUserOperationClaimQuery getByIdUserOperationClaimQuery)
    {
        GetByIdUserOperationClaimResponse result = await Sender.Send(getByIdUserOperationClaimQuery);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListUserOperationClaimQuery getListUserOperationClaimQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetListUserOperationClaimListItemDto> result = await Sender.Send(getListUserOperationClaimQuery);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateUserOperationClaimCommand createUserOperationClaimCommand)
    {
        CreatedUserOperationClaimResponse result = await Sender.Send(createUserOperationClaimCommand);
        return Created(uri: "", result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateUserOperationClaimCommand updateUserOperationClaimCommand)
    {
        UpdatedUserOperationClaimResponse result = await Sender.Send(updateUserOperationClaimCommand);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteUserOperationClaimCommand deleteUserOperationClaimCommand)
    {
        DeletedUserOperationClaimResponse result = await Sender.Send(deleteUserOperationClaimCommand);
        return Ok(result);
    }
}
