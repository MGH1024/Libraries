﻿using HealthChecks.UI.Client;
using MGH.Core.Infrastructure.MessageBroker.RabbitMq.Model;
using MGH.Core.Infrastructure.Persistence.EF.Models.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace MGH.Core.Infrastructure.HealthCheck;

public static class RegisterHealthCheck
{
    public static void AddInfrastructureHealthChecks<T>(this IServiceCollection services, IConfiguration configuration)
        where T : DbContext
    {
        var sqlConnectionString =
            configuration.GetSection(nameof(DatabaseConnection)).GetValue<string>("SqlConnection") ??
            throw new ArgumentNullException(nameof(DatabaseConnection.SqlConnection));

        var defaultConnection = configuration.GetSection("RabbitMq:DefaultConnection").Get<RabbitMqConnection>()
                                ?? throw new ArgumentNullException(nameof(RabbitMq.DefaultConnection));

        services.AddHealthChecks()
            .AddSqlServer(sqlConnectionString)
            .AddDbContextCheck<T>()
            .AddRabbitMQ(defaultConnection.HealthAddress);

        services.AddHealthChecksUI(setup =>
            {
                setup.SetEvaluationTimeInSeconds(10); // Set the evaluation time for health checks
                setup.MaximumHistoryEntriesPerEndpoint(60); // Set maximum history of health checks
                setup.SetApiMaxActiveRequests(1); // Set maximum API request concurrency
                setup.AddHealthCheckEndpoint("Health Check API", "/api/health"); // Map your health check API
            })
            .AddInMemoryStorage();
    }


    public static void AddHealthCheck(this WebApplication app)
    {
        app.MapHealthChecks("/api/health", new HealthCheckOptions()
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        app.UseHealthChecksUI(options =>
        {
            options.UIPath = "/health-ui";
            options.AddCustomStylesheet("./HealthCheck/custom.css");
        });
    }
}