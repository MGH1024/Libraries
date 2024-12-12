using Library.Infrastructure.Configurations.Base;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Configurations.Security;

public class PolicyConfiguration : IEntityTypeConfiguration<Policy>
{
    public void Configure(EntityTypeBuilder<Policy> builder)
    {
        builder.ToTable(DatabaseTableName.Policy, DatabaseSchema.SecuritySchema).HasKey(ea => ea.Id);

        builder.Property(ea => ea.Id).HasColumnName("Id").IsRequired();
        

        builder.Property(t => t.CreatedBy)
            .IsRequired()
            .HasMaxLength(maxLength: 64);

        builder.Property(t => t.CreatedAt)
            .IsRequired();

        builder.Property(t => t.UpdatedBy)
            .HasMaxLength(maxLength: 64);

        builder.Property(t => t.UpdatedAt)
            .IsRequired(false);

        builder.Property(t => t.DeletedBy)
            .HasMaxLength(maxLength: 64);

        builder.Property(t => t.DeletedAt)
            .IsRequired(false);

        builder.Property(a => a.CreatedBy)
            .HasDefaultValue("user");

        builder.Property(a => a.CreatedAt)
            .HasDefaultValueSql("GetDate()");
        builder.HasQueryFilter(ea => !ea.DeletedAt.HasValue);
        
        builder.HasMany(u => u.PolicyOperationClaims);
    }
}
