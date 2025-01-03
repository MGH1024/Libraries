using Asp.Versioning;
using AutoMapper;
using MediatR;
using MGH.Core.Endpoint.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Security.Application.Features.Auth.Commands.RefreshToken;
using Security.Application.Features.Auth.Commands.RegisterUser;
using Security.Application.Features.Auth.Commands.UserLogin;
using Security.Endpoint.Api.Profiles;

namespace Security.Endpoint.Api.Controllers.V1;

[ApiController]
[ApiVersion(1)]
[Route("{culture:CultureRouteConstraint}/api/v{v:apiVersion}/[Controller]")]
public class AuthController(ISender sender, IMapper mapper) : AppController(sender)
{
    /// <summary>
    /// security api login 
    /// </summary>
    /// <param name="userLoginCommandDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("Login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserLoginCommandResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] UserLoginCommandDto userLoginCommandDto, CancellationToken cancellationToken)
    {
        var command = mapper.Map<UserLoginCommand>(userLoginCommandDto);

        var response = await Sender.Send(command, cancellationToken);
        if (!response.IsSuccess)
            return BadRequest("Failed to login");

        if (!string.IsNullOrWhiteSpace(response.RefreshToken))
            SetRefreshTokenToCookie(response.RefreshToken, response.RefreshTokenExpiry);

        return Ok(response);
    }

    /// <summary>
    /// register new user
    /// </summary>
    /// <param name="registerUserCommandDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status201Created,Type = typeof(RegisterUserCommandResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommandDto registerUserCommandDto,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<RegisterUserCommand>(registerUserCommandDto);
        var response = await Sender.Send(command, cancellationToken);
        SetRefreshTokenToCookie(response.RefreshToken, response.RefreshTokenExpiry);
        return Created(uri: "", response);
    }

    /// <summary>
    /// get refresh token and set in the cookie
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RefreshTokenResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("RefreshToken")]
    public async Task<IActionResult> RefreshToken(CancellationToken cancellationToken)
    {
        var command = mapper.Map<RefreshTokenCommand>(GetRefreshTokenFromCookies());
        var response = await Sender.Send(command, cancellationToken);
        SetRefreshTokenToCookie(response.RefreshToken, response.RefreshTokenExpiry);
        return Created(uri: "", response);
    }

    /// <summary>
    /// revoke token
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("RevokeToken")]
    public async Task<IActionResult> RevokeToken([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)]
        string refreshToken, CancellationToken cancellationToken)
    {
        var token = refreshToken ?? GetRefreshTokenFromCookies();
        var revokeTokenCommand = token.ToRevokeTokenCommand();

        var result = await Sender.Send(revokeTokenCommand, cancellationToken);
        return Ok(result);
    }

    private string GetRefreshTokenFromCookies() => Request.Cookies["refreshToken"] ??
                                                   throw new ArgumentException(
                                                       "Refresh token is not found in request cookies.");

    private void SetRefreshTokenToCookie(string refreshToken, DateTime refreshTokenExpiry)
    {
        var cookieOptions = new CookieOptions() { HttpOnly = true, Expires = refreshTokenExpiry };
        Response.Cookies.Append(key: "refreshToken", refreshToken, cookieOptions);
    }
}