﻿using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public abstract class AppController(ISender sender) : ControllerBase
{
    protected readonly ISender Sender = sender;

    protected string IpAddress()
    {
        var ipAddress = Request.Headers.TryGetValue("X-Forwarded-For", out var header)
            ? header.ToString()
            : HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString()
              ?? throw new InvalidOperationException("IP address cannot be retrieved from request.");
        return ipAddress;
    }
}