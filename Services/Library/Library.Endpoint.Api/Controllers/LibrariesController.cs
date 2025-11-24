using MediatR;
using AutoMapper;
using MGH.Core.Endpoint.Web;
using Microsoft.AspNetCore.Mvc;
using MGH.Core.Application.Requests;
using Library.Application.Features.Libraries.Queries.GetList;
using Library.Application.Features.Libraries.Commands.EditLibrary;
using Library.Application.Features.Libraries.Commands.CreateLibrary;
using Library.Application.Features.Libraries.Commands.RemoveLibrary;
using Library.Application.Features.Libraries.Commands.AddLibraryStaff;
using Library.Application.Features.Libraries.Commands.RemoveLibraryStaff;

namespace Library.Endpoint.Api.Controllers;

[ApiController]
[Route("{culture:CultureRouteConstraint}/api/[Controller]")]
public class LibrariesController(ISender sender, IMapper mapper) : AppController(sender)
{

    [HttpGet()]
    public async Task<IActionResult> Get([FromQuery] PageRequest pageRequest)
    {
        var getLibraryListUserQuery = mapper.Map<GetLibraryListQuery>(pageRequest);
        return Ok(await Sender.Send(getLibraryListUserQuery));
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