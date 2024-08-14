using Application.Features.Auth.Commands.EnableEmailAuthenticator;
using Application.Features.Auth.Commands.EnableOtpAuthenticator;
using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.RefreshToken;
using Application.Features.Auth.Commands.Register;
using Application.Features.Auth.Commands.RevokeToken;
using Application.Features.Auth.Commands.VerifyEmailAuthenticator;
using Application.Features.Auth.Commands.VerifyOtpAuthenticator;
using MediatR;
using MGH.Core.Application.DTOs.Security;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Api.Controllers;

[ApiController]
[Route("{culture:CultureRouteConstraint}/api/[Controller]")]
public class AuthController : AppController
{
    private readonly WebApiConfiguration _configuration;
    private readonly ISender _sender;

    public AuthController(IConfiguration configuration, ISender sender) : base(sender)
    {
        _sender = sender;
        const string configurationSection = "WebAPIConfiguration";
        _configuration =
            configuration.GetSection(configurationSection).Get<WebApiConfiguration>()
            ?? throw new NullReferenceException($"\"{configurationSection}\" section cannot found in configuration.");
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] UserForLoginDto userForLoginDto,
        CancellationToken cancellationToken)
    {
        LoginCommand loginCommand = new() { UserForLoginDto = userForLoginDto, IpAddress = IpAddress() };
        LoggedResponse result = await _sender.Send(loginCommand, cancellationToken);

        if (result.RefreshTkn is not null)
            setRefreshTokenToCookie(result.RefreshTkn);

        return Ok(result.ToHttpResponse());
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] UserForRegisterDto userForRegisterDto,
        CancellationToken cancellationToken)
    {
        RegisterCommand registerCommand = new() { UserForRegisterDto = userForRegisterDto, IpAddress = IpAddress() };
        RegisteredResponse result = await _sender.Send(registerCommand, cancellationToken);
        setRefreshTokenToCookie(result.RefreshTkn);
        return Created(uri: "", result.AccessToken);
    }

    [HttpGet("RefreshToken")]
    public async Task<IActionResult> RefreshToken(CancellationToken cancellationToken)
    {
        RefreshTokenCommand refreshTokenCommand = new()
            { RefreshToken = getRefreshTokenFromCookies(), IpAddress = IpAddress() };
        RefreshedTokensResponse result = await _sender.Send(refreshTokenCommand, cancellationToken);
        setRefreshTokenToCookie(result.RefreshTkn);
        return Created(uri: "", result.AccessToken);
    }

    [HttpPut("RevokeToken")]
    public async Task<IActionResult> RevokeToken(
        [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)]
        string refreshToken, CancellationToken cancellationToken)
    {
        RevokeTokenCommand revokeTokenCommand = new()
            { Token = refreshToken ?? getRefreshTokenFromCookies(), IpAddress = IpAddress() };
        RevokedTokenResponse result = await _sender.Send(revokeTokenCommand, cancellationToken);
        return Ok(result);
    }

    [HttpGet("EnableEmailAuthenticator")]
    public async Task<IActionResult> EnableEmailAuthenticator(CancellationToken cancellationToken)
    {
        EnableEmailAuthenticatorCommand enableEmailAuthenticatorCommand =
            new()
            {
                UserId = GetUserIdFromRequest(),
                VerifyEmailUrlPrefix = $"{_configuration.ApiDomain}/Auth/VerifyEmailAuthenticator"
            };
        await _sender.Send(enableEmailAuthenticatorCommand, cancellationToken);

        return Ok();
    }

    [HttpGet("EnableOtpAuthenticator")]
    public async Task<IActionResult> EnableOtpAuthenticator(CancellationToken cancellationToken)
    {
        EnableOtpAuthenticatorCommand enableOtpAuthenticatorCommand = new() { UserId = GetUserIdFromRequest() };
        EnabledOtpAuthenticatorResponse result = await _sender.Send(enableOtpAuthenticatorCommand, cancellationToken);

        return Ok(result);
    }

    [HttpGet("VerifyEmailAuthenticator")]
    public async Task<IActionResult> VerifyEmailAuthenticator(
        [FromQuery] VerifyEmailAuthenticatorCommand verifyEmailAuthenticatorCommand,
        CancellationToken cancellationToken)
    {
        await _sender.Send(verifyEmailAuthenticatorCommand, cancellationToken);
        return Ok();
    }

    [HttpPost("VerifyOtpAuthenticator")]
    public async Task<IActionResult> VerifyOtpAuthenticator([FromBody] string authenticatorCode,
        CancellationToken cancellationToken)
    {
        VerifyOtpAuthenticatorCommand verifyEmailAuthenticatorCommand =
            new() { UserId = GetUserIdFromRequest(), ActivationCode = authenticatorCode };

        await _sender.Send(verifyEmailAuthenticatorCommand, cancellationToken);
        return Ok();
    }

    private string getRefreshTokenFromCookies() =>
        Request.Cookies["refreshToken"] ??
        throw new ArgumentException("Refresh token is not found in request cookies.");

    private void setRefreshTokenToCookie(RefreshTkn refreshToken)
    {
        CookieOptions cookieOptions = new() { HttpOnly = true, Expires = DateTime.UtcNow.AddDays(7) };
        Response.Cookies.Append(key: "refreshToken", refreshToken.Token, cookieOptions);
    }
}