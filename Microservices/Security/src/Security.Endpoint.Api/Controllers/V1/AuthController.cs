using Api.Extensions;
using Application.Features.Auth.Commands.Login;
using Asp.Versioning;
using AutoMapper;
using MediatR;
using MGH.Core.Application.DTOs.Security;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Api.Controllers.V1;

[ApiController]
[ApiVersion(1)]
[Route("{culture:CultureRouteConstraint}/api/v{v:apiVersion}/[Controller]")]
public class AuthController(ISender sender, IMapper mapper) : AppController(sender)
{
    /// <summary>
    /// login 
    /// </summary>
    /// <param name="userForLoginDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] UserForLoginDto userForLoginDto,
        CancellationToken cancellationToken)
    {
        var loginCommand = mapper.Map<LoginCommand>(userForLoginDto, opt => { opt.Items["IpAddress"] = IpAddress(); });

        var result = await Sender.Send(loginCommand, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest("Failed to login");

        if (result.RefreshTkn is not null)
            SetRefreshTokenToCookie(result.RefreshTkn);

        return Ok(result.ToHttpResponse());
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
        SetRefreshTokenToCookie(result.RefreshTkn);
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
        var refreshTokenCommand = ApiMapper.ToRefreshTokenCommand(GetRefreshTokenFromCookies(), IpAddress());
        var result = await Sender.Send(refreshTokenCommand, cancellationToken);
        SetRefreshTokenToCookie(result.RefreshTkn);
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
        [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] string refreshToken,
        CancellationToken cancellationToken)
    {
        var token = refreshToken ?? GetRefreshTokenFromCookies();
        var revokeTokenCommand = token.ToRevokeTokenCommand(IpAddress());

        var result = await Sender.Send(revokeTokenCommand, cancellationToken);
        return Ok(result);
    }

    private string GetRefreshTokenFromCookies() => Request.Cookies["refreshToken"] ??
                                                   throw new ArgumentException(
                                                       "Refresh token is not found in request cookies.");

    private void SetRefreshTokenToCookie(RefreshTkn refreshToken)
    {
        var cookieOptions = new CookieOptions() { HttpOnly = true, Expires = DateTime.UtcNow.AddDays(7) };
        Response.Cookies.Append(key: "refreshToken", refreshToken.Token, cookieOptions);
    }
}