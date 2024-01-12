using Persistence.Interceptors;
using Domain.Entities.Libraries;
using Application.Interfaces.Public;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Contexts;

public class LibraryDbContext : DbContext
{
    private readonly IDateTime _dateTime;

    public LibraryDbContext(DbContextOptions<LibraryDbContext> options, IDateTime dateTime) : base(options)
    {
        _dateTime = dateTime;
    }

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
        optionsBuilder.AddInterceptors(new AddAuditFieldsInterceptor(_dateTime));
    }

    private DbSet<Library> Libraries { get; set; }
}