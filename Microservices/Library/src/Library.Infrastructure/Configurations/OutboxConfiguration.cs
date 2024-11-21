using Domain.Entities.Libraries;
using Domain.Entities.Libraries.Constant;
using Domain.Entities.Libraries.ValueObjects;
using MGH.Core.Domain.Entity.Outboxes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nest;
using Persistence.Configurations.Base;

namespace Persistence.Configurations;

public class OutboxConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        //table
        builder.ToTable(DatabaseTableName.Outbox, DatabaseSchema.LibrarySchema);

        //fix fields section
        builder.Property(t => t.Id)
            .IsRequired();

        builder.Property(t => t.Content)
            .IsRequired()
            .HasMaxLength(4096);
        
        builder.Property(t => t.Error)
            .HasMaxLength(512);

        builder.Property(t => t.Type)
            .IsRequired();

        builder.Property(t => t.CreatedAt)
            .IsRequired();
    }
}