using Application.Features.Users.Commands.AddUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("{culture:CultureRouteConstraint}/api/[Controller]")]
public class UsersController(ISender sender) :AppController(sender)
{
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateUserCommand createUserCommand,CancellationToken cancellationToken)
    {
        var result = await Sender.Send(createUserCommand,cancellationToken);
        return Created(uri: "", result);
    }
}