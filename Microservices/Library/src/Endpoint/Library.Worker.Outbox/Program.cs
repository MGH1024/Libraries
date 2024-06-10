using System.Reflection;
using Domain.Entities.Libraries;
using Library.Worker.Outbox;
using MGH.Core.Infrastructure.ElasticSearch;
using MGH.Core.Infrastructure.ElasticSearch.Models;
using MGH.Core.Infrastructure.MessageBrokers;
using MGH.Core.Infrastructure.Persistence.Models;
using MGH.Core.Infrastructure.Public;
using Microsoft.EntityFrameworkCore;
using Nest;
using Persistence.Contexts;
using Persistence.Repositories;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});

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
builder.Services.AddSingleton<IOutBoxRepository, OutBoxRepository>();
builder.Services.AddTransient<IDateTime, DateTimeService>();


const string configurationSection = "ElasticSearchConfig";
var setting =
    builder.Configuration.GetSection(configurationSection).Get<ElasticSearchConfig>()
    ?? throw new NullReferenceException($"\"{configurationSection}\" " +
                                        $"section cannot found in configuration.");

var connectionSettings = new ConnectionSettings(new Uri(setting.ConnectionString));
var client = new ElasticClient(connectionSettings);
builder.Services.AddSingleton(client);
builder.Services.AddSingleton<IElasticSearch, ElasticSearchService>(x => new ElasticSearchService(client));

foreach (var i in setting.Indices)
{
    if (!(await client.Indices.ExistsAsync(i.IndexName)).Exists)
    {
        await client.Indices.CreateAsync(i.IndexName, selector: se =>
            se.Settings(a => a.NumberOfReplicas(i.ReplicaCount)
                    .NumberOfShards(i.ShardNumber))
                .Aliases(x => x.Alias(i.AliasName))
        );
    }
}

builder.Services.AddTransient(typeof(IMessageSender<>), typeof(RabbitMqService<>));
builder.Services.AddHostedService<Worker>();
var host = builder.Build();
host.Run();