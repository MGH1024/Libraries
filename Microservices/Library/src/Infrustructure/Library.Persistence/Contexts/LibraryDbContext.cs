using Domain.Entities.Libraries;
using MGH.Core.Domain.Outboxes;
using MGH.Core.Infrastructure.Persistence.Persistence.Interceptors;
using MGH.Core.Infrastructure.Public;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Contexts;

public class LibraryDbContext(DbContextOptions<LibraryDbContext> options, IDateTime dateTime)
    : DbContext(options)
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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new AddAuditFieldsInterceptor(dateTime));
        
    }

    private DbSet<Library> Libraries { get; set; }
    private DbSet<OutboxMessage> OutboxMessages { get; set; }
}