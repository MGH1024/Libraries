using Domain.Entities.Libraries;
using MGH.Core.Domain.Outboxes;
using MGH.Core.Infrastructure.Public;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Interceptors;
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
    
    public DbSet<EmailAuthenticator> EmailAuthenticators { get; set; }
    public DbSet<OperationClaim> OperationClaims { get; set; }
    public DbSet<OtpAuthenticator> OtpAuthenticators { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
}