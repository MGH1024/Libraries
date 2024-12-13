using Library.Application.Features.Libraries.Commands.AddLibraryStaff;
using Library.Application.Features.Libraries.Commands.CreateLibrary;
using Library.Application.Features.Libraries.Commands.EditLibrary;
using Library.Application.Features.Libraries.Commands.RemoveLibrary;
using Library.Application.Features.Libraries.Commands.RemoveLibraryStaff;
using Library.Application.Features.Libraries.Queries.GetList;
using MediatR;
using MGH.Core.Application.Requests;
using MGH.Core.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Library.Endpoint.Api.Controllers;

[ApiController]
[Route("{culture:CultureRouteConstraint}/api/[Controller]")]
public class LibrariesController(ISender sender) : AppController(sender)
{
    
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetLibraryListQuery getLibraryListUserQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetLibraryListDto> result = await Sender.Send(getLibraryListUserQuery);
        return Ok(result);
    }
    
    
    [HttpPost("create-library")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> InsertAsync([FromBody] CreateLibraryCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Sender.Send(command, cancellationToken));
    }

    [HttpPost("create-staff")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddStaffAsync([FromBody] CreateLibraryStaffCommand command, CancellationToken cancellationToken)
    {
        await Sender.Send(command, cancellationToken);
        return Ok();
    }

    [HttpPost("delete-staff")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveStaffAsync([FromBody] DeleteLibraryStaffCommand command, CancellationToken cancellationToken)
    {
        await Sender.Send(command, cancellationToken);
        return Ok();
    }

    [HttpPut("update-library")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateLibraryAsync([FromBody] UpdateLibraryCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Sender.Send(command, cancellationToken));
    }

    [HttpPut("update-library-with-staves")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateLibraryWithStavesAsync([FromBody] UpdateLibraryWithStavesCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Sender.Send(command, cancellationToken));
    }

    [HttpDelete("delete-library")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveLibraryWithStavesAsync([FromBody] DeleteLibraryCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Sender.Send(command, cancellationToken));
    }
}