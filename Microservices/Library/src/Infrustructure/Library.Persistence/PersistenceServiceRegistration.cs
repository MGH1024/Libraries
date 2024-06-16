using Domain.Entities.Libraries;
using Domain.Entities.Libraries.Factories;
using Domain.Entities.Libraries.Policies;
using MGH.Core.Infrastructure.Persistence.Base;
using MGH.Core.Infrastructure.Persistence.Models;
using MGH.Core.Infrastructure.Persistence.Models.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Persistence.Contexts;
using Persistence.Repositories;
using Persistence.UOW;

namespace Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceService(this IServiceCollection services , IConfiguration configuration)
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
        var postgresConfig = configuration
            .GetSection(nameof(DatabaseConnection))
            .Get<DatabaseConnection>()
            .PostgresConnection;
        services
            .AddDbContext<LibraryDbContext>(options =>
                options.UseNpgsql(postgresConfig, a =>
                    {
                        a.EnableRetryOnFailure();
                        //a.MigrationsAssembly("Library.Api");
                    })
                    .AddInterceptors()
                    .LogTo(Console.Write, LogLevel.Information));

        services.AddHealthChecks().AddDbContextCheck<LibraryDbContext>();
        services.AddDbContext<LibraryDbContext>(options => options.UseInMemoryDatabase("LibraryMicroService"));
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<ILibraryRepository, LibraryRepository>();
        services.AddScoped<IOutBoxRepository, OutBoxRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ILibraryFactory, LibraryFactory>();
        services.AddScoped<ILibraryPolicy, DistrictPolicy>();
        return services;

        #endregion
    }
    
}