using MGH.Core.Infrastructure.Securities.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Security.Infrastructure.Configurations.Base;

namespace Security.Infrastructure.Configurations.Security;

public class PolicyConfiguration : IEntityTypeConfiguration<Policy>
{
    public void Configure(EntityTypeBuilder<Policy> builder)
    {
        builder.ToTable(DatabaseTableName.Policy, DatabaseSchema.SecuritySchema).HasKey(ea => ea.Id);
        builder.Property(ea => ea.Id).HasColumnName("Id").IsRequired();
        builder.HasQueryFilter(ea => !ea.DeletedAt.HasValue);
        builder.HasMany(u => u.PolicyOperationClaims);
    }
}
