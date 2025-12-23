using AutoMapper;
using Library.Application.Features.PublicLibraries.Commands.Add;
using Library.Application.Features.PublicLibraries.Commands.AddStaff;
using Library.Application.Features.PublicLibraries.Commands.Remove;
using Library.Application.Features.PublicLibraries.Commands.RemoveStaff;
using Library.Application.Features.PublicLibraries.Commands.Update;
using Library.Application.Features.PublicLibraries.Queries.GetById;
using Library.Application.Features.PublicLibraries.Queries.GetList;
using MediatR;
using MGH.Core.Application.Requests;
using MGH.Core.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Library.Endpoint.Api.Controllers;

/// <summary>
/// Manages public libraries and their staff.
/// </summary>
[ApiController]
[Route("{culture:CultureRouteConstraint}/api/public-libraries")]
[Produces("application/json")]
public class PublicLibrariesController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public PublicLibrariesController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets a public library by identifier.
    /// I use ActionResult to return response type and controller does not concerned with BL
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GetByIdQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetByIdQueryResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _sender.Send(
            new GetByIdQuery(id),
            cancellationToken);
    }

    /// <summary>
    /// Gets a paginated list of public libraries.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(GetListResponse<GetListQueryResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetListResponse<GetListQueryResponse>>> GetAll(
     [FromQuery] PageRequest pageRequest,
     CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetListQuery>(pageRequest);
        return await _sender.Send(query, cancellationToken);
    }


    /// <summary>
    /// Creates a new public library.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] AddCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>
    /// Updates an existing public library.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateCommand command,
        CancellationToken cancellationToken)
    {
        command.Id = id;
        await _sender.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Deletes a public library.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _sender.Send(new RemoveCommand { Id = id }, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Adds a staff member to a public library.
    /// </summary>
    [HttpPost("{id:guid}/staff")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddStaff(
        Guid id,
        [FromBody] AddStaffCommand command,
        CancellationToken cancellationToken)
    {
        command.LibraryId = id;
        await _sender.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Removes a staff member from a public library.
    /// </summary>
    [HttpDelete("{id:guid}/staff")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveStaff(
        Guid id,
        [FromBody] RemoveStaffCommand command,
        CancellationToken cancellationToken)
    {
        command.LibraryId = id;
        await _sender.Send(command, cancellationToken);
        return NoContent();
    }
}
