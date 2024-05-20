using Application.Models.Database;
using Domain.Entities.Libraries;
using Domain.Entities.Libraries.Factories;
using Domain.Entities.Libraries.Policies;
using MGH.Core.Infrastructure.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Persistence.Contexts;
using Persistence.Repositories;

namespace Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceService(this WebApplicationBuilder builder)
    {
        #region sqlserver

        // var sqlConfig = configuration
        //     .GetSection(nameof(DatabaseConnection))
        //     .Get<DatabaseConnection>()
        //     .SqlConnection;
        //
        // services
        //     .AddDbContext<LibraryDbContext>(options =>
        //         options.UseSqlServer(sqlConfig, a =>
        //             {
        //                 a.EnableRetryOnFailure();
        //                 //a.MigrationsAssembly("Library.Api");
        //             })
        //             .AddInterceptors()
        //             .LogTo(Console.Write, LogLevel.Information));

        #endregion

        #region postgres

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        var postgresConfig = builder.Configuration
            .GetSection(nameof(DatabaseConnection))
            .Get<DatabaseConnection>()
            .PostgresConnection;
        builder.Services
            .AddDbContext<LibraryDbContext>(options =>
                options.UseNpgsql(postgresConfig, a =>
                    {
                        a.EnableRetryOnFailure();
                        //a.MigrationsAssembly("Library.Api");
                    })
                    .AddInterceptors()
                    .LogTo(Console.Write, LogLevel.Information));

        builder.Services.AddHealthChecks().AddDbContextCheck<LibraryDbContext>();
        builder.Services.AddDbContext<LibraryDbContext>(options => options.UseInMemoryDatabase("LibraryMicroService"));
        builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddScoped<ILibraryRepository, LibraryRepository>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<ILibraryFactory, LibraryFactory>();
        builder.Services.AddScoped<ILibraryPolicy, DistrictPolicy>();
        return builder.Services;

        #endregion
    }
}