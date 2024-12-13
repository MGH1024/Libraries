using MGH.Core.Domain.Entity.Outboxes;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Contexts;

public class LibraryDbContext(DbContextOptions<LibraryDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LibraryDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<string>().HaveMaxLength(64);
        base.ConfigureConventions(configurationBuilder);
    }

    private DbSet<Domain.Entities.Libraries.Library> Libraries { get; set; }
    private DbSet<OutboxMessage> OutboxMessages { get; set; }
}