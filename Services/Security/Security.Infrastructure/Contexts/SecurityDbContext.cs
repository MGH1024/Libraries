using MGH.Core.Domain.BaseModels;
using MGH.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Security.Infrastructure.Contexts;

public class SecurityDbContext(DbContextOptions<SecurityDbContext> options) :
    DbContext(options)
{
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<string>().HaveMaxLength(64);
        base.ConfigureConventions(configurationBuilder);
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SecurityDbContext).Assembly);
        //ApplyAuditFieldConfiguration(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    private void ApplyAuditFieldConfiguration(ModelBuilder modelBuilder)
    {
        var auditEntityTypes = modelBuilder.Model.GetEntityTypes()
            .Where(e => e.ClrType.IsSubclassOf(typeof(Entity<>)))
            .ToList();

        foreach (var entityType in auditEntityTypes)
        {
            modelBuilder.Entity(entityType.ClrType)
                .Property("CreatedBy")
                .IsRequired()
                .HasDefaultValue("admin_seed")
                .HasMaxLength(maxLength: 64);

            modelBuilder.Entity(entityType.ClrType)
                .Property("CreatedAt")
                .HasDefaultValueSql("GETUTCDATE()")
                .IsRequired();

            modelBuilder.Entity(entityType.ClrType)
                .Property("CreatedByIp")
                .HasMaxLength(maxLength: 32)
                .IsRequired();

            modelBuilder.Entity(entityType.ClrType)
                .Property("UpdatedBy")
                .HasMaxLength(maxLength: 64);

            modelBuilder.Entity(entityType.ClrType)
                .Property("UpdatedAt")
                .IsRequired(false);

            modelBuilder.Entity(entityType.ClrType)
                .Property("UpdatedByIp")
                .HasMaxLength(maxLength: 32);

            modelBuilder.Entity(entityType.ClrType)
                .Property("DeletedBy")
                .HasMaxLength(maxLength: 64);

            modelBuilder.Entity(entityType.ClrType)
                .Property("DeletedAt")
                .IsRequired(false);

            modelBuilder.Entity(entityType.ClrType)
                .Property("DeletedByIp")
                .HasMaxLength(maxLength: 32);
        }
    }


    public DbSet<OperationClaim> OperationClaims { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
}