using MGH.Core.Domain.Events;
using Library.Domain.Lendings;
using Library.Domain.Libraries;
using Microsoft.EntityFrameworkCore;
using MGH.Core.Infrastructure.Persistence.Entities;

namespace Library.Infrastructure.Contexts;

public class PublicLibraryDbContext(DbContextOptions<PublicLibraryDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuditLog).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PublicLibraryDbContext).Assembly);
        modelBuilder.Ignore<DomainEvent>();
        base.OnModelCreating(modelBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<string>().HaveMaxLength(64);
        base.ConfigureConventions(configurationBuilder);
    }

    private DbSet<PublicLibrary> Libraries { get; set; }
    private DbSet<Lending> Lendings { get; set; }
    private DbSet<OutboxMessage> OutboxMessages { get; set; }
    private DbSet<AuditLog> AuditLogs { get; set; }
}