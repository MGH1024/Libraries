using MGH.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Security.Infrastructure.Configurations.Base;

namespace Security.Infrastructure.Configurations.AuditLogs;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable(DatabaseTableName.AuditLog, DatabaseSchema.Log);

        builder.HasKey(a => a.Id);

        builder.Property(a => a.TableName)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(a => a.Username)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(a => a.Action)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(a => a.Timestamp)
            .IsRequired();

        builder.Property(a => a.BeforeData)
            .HasMaxLength(4096);

        builder.Property(a => a.AfterData)
            .HasMaxLength(4096);
    }
}