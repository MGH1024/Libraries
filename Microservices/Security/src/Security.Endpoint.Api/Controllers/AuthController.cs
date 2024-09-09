using MediatR;
using Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using MGH.Core.Application.DTOs.Security;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Api.Controllers;

[ApiController]
[Route("{culture:CultureRouteConstraint}/api/[Controller]")]
public class AuthController(ISender sender) : AppController(sender)
{
    /// <summary>
    /// login 
    /// </summary>
    /// <param name="userForLoginDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] UserForLoginDto userForLoginDto, CancellationToken cancellationToken)
    {
        var loginCommand = userForLoginDto.ToLoginCommand(IpAddress());

        var result = await Sender.Send(loginCommand, cancellationToken);
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
    public async Task<IActionResult> Register([FromBody] UserForRegisterDto userForRegisterDto, CancellationToken cancellationToken)
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
    public async Task<IActionResult> RevokeToken([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] string refreshToken,
        CancellationToken cancellationToken)
    {
        var token = refreshToken ?? GetRefreshTokenFromCookies();
        var revokeTokenCommand = token.ToRevokeTokenCommand(IpAddress());

        var result = await Sender.Send(revokeTokenCommand, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// enable email authenticator
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("EnableEmailAuthenticator")]
    public async Task<IActionResult> EnableEmailAuthenticator(CancellationToken cancellationToken)
    {
        var enableEmailAuthenticatorCommand = ApiMapper.ToEnableEmailAuthenticatorCommand(GetUserIdFromRequest());
        await Sender.Send(enableEmailAuthenticatorCommand, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// enable otp authenticator
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("EnableOtpAuthenticator")]
    public async Task<IActionResult> EnableOtpAuthenticator(CancellationToken cancellationToken)
    {
        var enableOtpAuthenticatorCommand = ApiMapper.ToEnableOtpAuthenticatorCommand(GetUserIdFromRequest());
        var result = await Sender.Send(enableOtpAuthenticatorCommand, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// verify email authenticator
    /// </summary>
    /// <param name="verifyEmailAuthenticatorDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("VerifyEmailAuthenticator")]
    public async Task<IActionResult> VerifyEmailAuthenticator([FromQuery] VerifyEmailAuthenticatorDto verifyEmailAuthenticatorDto,
        CancellationToken cancellationToken)
    {
        var verifyEmailAuthenticatorCommand = verifyEmailAuthenticatorDto.ToVerifyEmailAuthenticatorCommand();
        await Sender.Send(verifyEmailAuthenticatorCommand, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// verify otp authenticator
    /// </summary>
    /// <param name="authenticatorCode"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("VerifyOtpAuthenticator")]
    public async Task<IActionResult> VerifyOtpAuthenticator([FromBody] string authenticatorCode, CancellationToken cancellationToken)
    {
        var verifyEmailAuthenticatorCommand = authenticatorCode.ToVerifyOtpAuthenticatorCommand(GetUserIdFromRequest());
        await Sender.Send(verifyEmailAuthenticatorCommand, cancellationToken);
        return Ok();
    }

    private string GetRefreshTokenFromCookies() => Request.Cookies["refreshToken"] ??
                                                   throw new ArgumentException("Refresh token is not found in request cookies.");

    private void SetRefreshTokenToCookie(RefreshTkn refreshToken)
    {
        var cookieOptions = new CookieOptions() { HttpOnly = true, Expires = DateTime.UtcNow.AddDays(7) };
        Response.Cookies.Append(key: "refreshToken", refreshToken.Token, cookieOptions);
    }
}