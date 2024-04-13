using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Libraries.Commands.AddLibraryStaff;
using Application.Features.Libraries.Commands.CreateLibrary;
using Application.Features.Libraries.Commands.EditLibrary;
using Application.Features.Libraries.Commands.RemoveLibrary;
using Application.Features.Libraries.Commands.RemoveLibraryStaff;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LibrariesController(ISender sender) : AppController(sender)
{
    [HttpPost("create-library")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> InsertAsync([FromBody] CreateLibraryCommand command, CancellationToken
        cancellationToken)
    {
        return Ok(await Sender.Send(command, cancellationToken));
    }

    [HttpPost("create-staff")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddStaffAsync([FromBody] CreateLibraryStaffCommand command, CancellationToken
        cancellationToken)
    {
        await Sender.Send(command, cancellationToken);
        return Ok();
    }

    [HttpPost("delete-staff")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveStaffAsync([FromBody] DeleteLibraryStaffCommand command, CancellationToken
        cancellationToken)
    {
        await Sender.Send(command, cancellationToken);
        return Ok();
    }

    [HttpPut("update-library")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateLibraryAsync([FromBody] UpdateLibraryCommand command, CancellationToken
        cancellationToken)
    {
        return Ok(await Sender.Send(command, cancellationToken));
    }

    [HttpPut("update-library-with-staves")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateLibraryWithStavesAsync([FromBody] UpdateLibraryWithStavesCommand command,
        CancellationToken cancellationToken)
    {
        return Ok(await Sender.Send(command, cancellationToken));
    }

    [HttpDelete("delete-library")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveLibraryWithStavesAsync([FromBody] DeleteLibraryCommand command,
        CancellationToken cancellationToken)
    {
        return Ok(await Sender.Send(command, cancellationToken));
    }
}