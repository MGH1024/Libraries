using Api.Profiles;
using MediatR;
using AutoMapper;
using Quartz.Util;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.RefreshToken;
using Application.Features.Auth.Commands.RegisterUser;

namespace Api.Controllers.V1;

[ApiController]
[ApiVersion(1)]
[Route("{culture:CultureRouteConstraint}/api/v{v:apiVersion}/[Controller]")]
public class AuthController(ISender sender, IMapper mapper) : AppController(sender)
{
    /// <summary>
    /// security api login 
    /// </summary>
    /// <param name="loginCommandDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("Login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginCommandDto loginCommandDto, CancellationToken
        cancellationToken)
    {
        var loginCommand = mapper.Map<LoginCommand>(loginCommandDto);

        var result = await Sender.Send(loginCommand, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest("Failed to login");

        if (!result.RefreshToken.IsNullOrWhiteSpace())
            SetRefreshTokenToCookie(result.RefreshToken, result.RefreshTokenExpiry);

        return Ok(result);
    }

    /// <summary>
    /// register new user
    /// </summary>
    /// <param name="registerUserCommandDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommandDto registerUserCommandDto,
        CancellationToken cancellationToken)
    {
        var registerCommand = mapper.Map<RegisterUserCommand>(registerUserCommandDto);
        var result = await Sender.Send(registerCommand, cancellationToken);
        SetRefreshTokenToCookie(result.RefreshToken, result.RefreshTokenExpiry);
        return Created(uri: "", result);
    }

    /// <summary>
    /// get refresh token and set in the cookie
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("RefreshToken")]
    public async Task<IActionResult> RefreshToken(CancellationToken cancellationToken)
    {
        var refreshTokenCommand = mapper.Map<RefreshTokenCommand>(GetRefreshTokenFromCookies());
        var refreshTokenResponse = await Sender.Send(refreshTokenCommand, cancellationToken);
        SetRefreshTokenToCookie(refreshTokenResponse.RefreshToken, refreshTokenResponse.RefreshTokenExpiry);
        return Created(uri: "", refreshTokenResponse);
    }

    /// <summary>
    /// revoke token
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("RevokeToken")]
    public async Task<IActionResult> RevokeToken(
        [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)]
        string refreshToken,
        CancellationToken cancellationToken)
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