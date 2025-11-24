using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MGH.Core.Application.Requests;
using MGH.Core.Application.Responses;
using Library.Application.Features.PublicLibraries.Queries.GetById;
using Library.Application.Features.PublicLibraries.Queries.GetList;
using Library.Application.Features.PublicLibraries.Commands.RemoveStaff;
using Library.Application.Features.PublicLibraries.Commands.Remove;
using Library.Application.Features.PublicLibraries.Commands.Add;
using Library.Application.Features.PublicLibraries.Commands.AddStaff;
using Library.Application.Features.PublicLibraries.Commands.Update;

namespace Library.Endpoint.Api.Controllers;

[ApiController]
[Route("{culture:CultureRouteConstraint}/api/[controller]")]
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

    // GET /{culture}/api/PublicLibraries/{id}
    [HttpGet("{id:guid}", Name = "GetPublicLibraryById")]
    [ProducesResponseType(typeof(GetByIdQuery), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetByIdQuery { Id = id };
        var result = await _sender.Send(query);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    // GET /{culture}/api/PublicLibraries?page=1&pageSize=10
    [HttpGet(Name = "GetPublicLibraries")]
    [ProducesResponseType(typeof(GetListResponse<GetListQuery>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] PageRequest pageRequest)
    {
        var query = _mapper.Map<GetListQuery>(pageRequest);
        var result = await _sender.Send(query);
        return Ok(result);
    }

    // POST /{culture}/api/PublicLibraries/create-library
    [HttpPost("create-library")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateLibrary([FromBody] AddCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return Ok(result);
    }

    // POST /{culture}/api/PublicLibraries/create-staff
    [HttpPost("create-staff")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddStaff([FromBody] AddStaffCommand command, CancellationToken cancellationToken)
    {
        await _sender.Send(command, cancellationToken);
        return Ok();
    }

    // POST /{culture}/api/PublicLibraries/delete-staff
    [HttpPost("delete-staff")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveStaff([FromBody] RemoveStaffCommand command, CancellationToken cancellationToken)
    {
        await _sender.Send(command, cancellationToken);
        return Ok();
    }

    // PUT /{culture}/api/PublicLibraries/update-library
    [HttpPut("update-library")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateLibrary([FromBody] EditCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return Ok(result);
    }

    // PUT /{culture}/api/PublicLibraries/update-library-with-staves
    [HttpPut("update-library-with-staves")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateLibraryWithStaves([FromBody] UpdateLibraryWithStavesCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return Ok(result);
    }

    // DELETE /{culture}/api/PublicLibraries/delete-library
    [HttpDelete("delete-library")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveLibrary([FromBody] RemoveCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return Ok(result);
    }
}
