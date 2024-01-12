﻿using Application.Models.Database;
using Domain.Entities.Libraries;
using Domain.Entities.Libraries.Factories;
using Domain.Entities.Libraries.Policies;
using MGH.Core.Persistence.UnitOfWork;
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
    public static IServiceCollection AddPersistenceService(this IServiceCollection services,
        IConfiguration configuration)
    {
        var sqlConfig = configuration
            .GetSection(nameof(DatabaseConnection))
            .Get<DatabaseConnection>()
            .SqlConnection;

        services.AddHealthChecks()
            .AddDbContextCheck<LibraryDbContext>();

        services
            .AddDbContext<LibraryDbContext>(options =>
                options.UseSqlServer(sqlConfig, a =>
                    {
                        a.EnableRetryOnFailure();
                        a.MigrationsAssembly("LibraryMicroservice.Api");
                    })
                    .AddInterceptors()
                    .LogTo(Console.Write, LogLevel.Information));

        services.AddDbContext<LibraryDbContext>(options => options.UseInMemoryDatabase("LibraryMicroService"));
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddScoped<ILibraryRepository, LibraryRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ILibraryFactory, LibraryFactory>();
        services.AddScoped<ILibraryPolicy, DistrictPolicy>();
        return services;
    }
}