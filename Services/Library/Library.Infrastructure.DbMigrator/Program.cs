using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Library.Infrastructure.Contexts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.MigrationTool;
public class Program
{
    public static async Task Main(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                          ?? "Development";

        var host = CreateHostBuilder(args, environment).Build();

        await ApplyMigrationsAsync(host);
    }

    private static IHostBuilder CreateHostBuilder(string[] args, string environment)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.Sources.Clear();
                config
                    .AddJsonFile("appsettings.json", optional: false)
                    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();
            })
            .ConfigureServices((context, services) =>
            {
                ConfigureDatabase(services, context.Configuration);
            });
    }

    private static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");
        Console.WriteLine(connectionString);
        services.AddDbContext<PublicLibraryDbContext>(options =>
        {
            options.UseSqlServer(connectionString,
                sql => sql.MigrationsAssembly(typeof(PublicLibraryDbContext).Assembly.FullName));
        });
    }

    private static async Task ApplyMigrationsAsync(IHost host)
    {
        using var scope = host.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PublicLibraryDbContext>();

        Console.WriteLine("Applying migrations...");
        await dbContext.Database.MigrateAsync();
        Console.WriteLine("Migration complete.");
    }
}
