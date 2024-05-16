using System.Text.Json;
using MediatR;
using MGH.Core.CrossCutting.Logging;

using Microsoft.AspNetCore.Http;

namespace MGH.Core.Application.Pipelines.Logging;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ILoggableRequest
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LoggingBehavior( IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        List<LogParameter> logParameters =
            new()
            {
                new LogParameter { Type = request.GetType().Name, Value = request }
            };

        LogDetail logDetail =
            new()
            {
                MethodName = next.Method.Name,
                Parameters = logParameters,
                User = _httpContextAccessor.HttpContext.User.Identity?.Name ?? "?"
            };

        //_loggerServiceBase.Info(JsonSerializer.Serialize(logDetail));
        return await next();
    }
}
