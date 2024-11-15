using Domain.Entities.Libraries;
using MGH.Core.Domain.Outboxes;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Contexts;

public class LibraryDbContext(DbContextOptions<LibraryDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LibraryDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<string>().HaveMaxLength(128);
        base.ConfigureConventions(configurationBuilder);
    }

    private DbSet<Library> Libraries { get; set; }
    private DbSet<OutboxMessage> OutboxMessages { get; set; }
}