﻿using Application.Features.OperationClaims.Commands.Create;
using Application.Features.OperationClaims.Commands.Delete;
using Application.Features.OperationClaims.Commands.Update;
using Application.Features.OperationClaims.Queries.GetById;
using Application.Features.OperationClaims.Queries.GetList;
using Asp.Versioning;
using MediatR;
using MGH.Core.Application.Requests;
using MGH.Core.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.V1;

[ApiController]
[ApiVersion(1)]
[Route("{culture:CultureRouteConstraint}/api/v{v:apiVersion}/[Controller]")]
public class OperationClaimsController(ISender sender) : AppController(sender)
{
    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById([FromRoute] GetByIdOperationClaimQuery getByIdOperationClaimQuery,
        CancellationToken cancellationToken)
    {
        GetByIdOperationClaimResponse result = await Sender.Send(getByIdOperationClaimQuery, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest, CancellationToken cancellationToken)
    {
        GetListOperationClaimQuery getListOperationClaimQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetListOperationClaimListItemDto> result =
            await Sender.Send(getListOperationClaimQuery, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateOperationClaimCommand createOperationClaimCommand,
        CancellationToken cancellationToken)
    {
        CreatedOperationClaimResponse result = await Sender.Send(createOperationClaimCommand, cancellationToken);
        return Created(uri: "", result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateOperationClaimCommand updateOperationClaimCommand,
        CancellationToken cancellationToken)
    {
        UpdatedOperationClaimResponse result = await Sender.Send(updateOperationClaimCommand, cancellationToken);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteOperationClaimCommand deleteOperationClaimCommand,
        CancellationToken cancellationToken)
    {
        DeletedOperationClaimResponse result = await Sender.Send(deleteOperationClaimCommand, cancellationToken);
        return Ok(result);
    }
}