using MediatR;
using AutoMapper;
using Quartz.Util;
using Asp.Versioning;
using Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using MGH.Core.Application.DTOs.Security;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Application.Features.Auth.Commands.Login;

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
            SetRefreshTokenToCookie(result.RefreshToken,result.RefreshTokenExpiry);

        return Ok(result);
    }

    /// <summary>
    /// register new user
    /// </summary>
    /// <param name="userForRegisterDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] UserForRegisterDto userForRegisterDto,
        CancellationToken cancellationToken)
    {
        var registerCommand = userForRegisterDto.ToRegisterCommand(IpAddress());

        var result = await Sender.Send(registerCommand, cancellationToken);
        SetRefreshTokenToCookie(result.RefreshTkn.Token,result.RefreshTkn.Expires);
        return Created(uri: "", result.AccessToken);
    }

    /// <summary>
    /// get refresh token and set in the cookie
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("RefreshToken")]
    public async Task<IActionResult> RefreshToken(CancellationToken cancellationToken)
    {
        var refreshTokenCommand = ApiMapper.ToRefreshTokenCommand(GetRefreshTokenFromCookies());
        var result = await Sender.Send(refreshTokenCommand, cancellationToken);
        SetRefreshTokenToCookie(result.RefreshTkn.Token,result.RefreshTkn.Expires);
        return Created(uri: "", result.AccessToken);
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