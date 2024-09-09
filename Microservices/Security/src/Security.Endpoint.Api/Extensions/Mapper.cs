using Application.Features.Auth.Commands.EnableEmailAuthenticator;
using Application.Features.Auth.Commands.EnableOtpAuthenticator;
using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.RefreshToken;
using Application.Features.Auth.Commands.Register;
using Application.Features.Auth.Commands.RevokeToken;
using Application.Features.Auth.Commands.VerifyEmailAuthenticator;
using Application.Features.Auth.Commands.VerifyOtpAuthenticator;
using MGH.Core.Application.DTOs.Security;
using Microsoft.AspNetCore.Identity.Data;

namespace Api.Extensions;

public static class ApiMapper
{
    public static LoginCommand ToLoginCommand(this UserForLoginDto userForLoginDto, string ipAddress)
    {
        return new LoginCommand
        {
            UserForLoginDto = userForLoginDto,
            IpAddress = ipAddress
        };
    }

    public static RegisterCommand ToRegisterCommand(this UserForRegisterDto userForRegisterDto, string ipAddress)
    {
        return new RegisterCommand
        {
            UserForRegisterDto = userForRegisterDto,
            IpAddress = ipAddress
        };
    }

    public static RefreshTokenCommand ToRefreshTokenCommand(string refreshToken, string ipAddress)
    {
        return new RefreshTokenCommand
        {
            RefreshToken = refreshToken,
            IpAddress = ipAddress
        };
    }


    public static RevokeTokenCommand ToRevokeTokenCommand(this string refreshToken, string ipAddress)
    {
        return new RevokeTokenCommand
        {
            Token = refreshToken,
            IpAddress = ipAddress
        };
    }


    public static EnableEmailAuthenticatorCommand ToEnableEmailAuthenticatorCommand(int userId)
    {
        return new EnableEmailAuthenticatorCommand
        {
            UserId = userId
        };
    }


    public static EnableOtpAuthenticatorCommand ToEnableOtpAuthenticatorCommand(int userId)
    {
        return new EnableOtpAuthenticatorCommand
        {
            UserId = userId
        };
    }

    public static VerifyEmailAuthenticatorCommand ToVerifyEmailAuthenticatorCommand(this VerifyEmailAuthenticatorDto verifyEmailAuthenticatorDto)
    {
        return new VerifyEmailAuthenticatorCommand
        {
            VerifyEmailAuthenticatorDto = verifyEmailAuthenticatorDto
        };
    }

    public static VerifyOtpAuthenticatorCommand ToVerifyOtpAuthenticatorCommand(this string authenticationCode, int userId)
    {
        return new VerifyOtpAuthenticatorCommand
        {
            ActivationCode = authenticationCode,
            UserId = userId,
        };
    }
}