using Library.Infrastructure.Configurations.Base;
using MGH.Core.Domain.Entity.Outboxes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Configurations;

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