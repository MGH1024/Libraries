using MGH.Core.Domain.Entity.Logs;
using Microsoft.EntityFrameworkCore;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Persistence.Contexts;

public class SecurityDbContext(DbContextOptions<SecurityDbContext> options) : 
    DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SecurityDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<string>().HaveMaxLength(128);
        base.ConfigureConventions(configurationBuilder);
    }
    

    public DbSet<OperationClaim> OperationClaims { get; set; }
    public DbSet<RefreshTkn> RefreshTokens { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
}