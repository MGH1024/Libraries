using MGH.Core.Infrastructure.Securities.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Configurations.Base;

namespace Persistence.Configurations.Security;

public class PolicyOperationClaimConfiguration : IEntityTypeConfiguration<PolicyOperationClaim>
{
    public void Configure(EntityTypeBuilder<PolicyOperationClaim> builder)
    {
        builder.ToTable(DatabaseTableName.PolicyOperationClaim, DatabaseSchema.SecuritySchema).HasKey(ea => ea.Id);
        builder.Property(ea => ea.Id).HasColumnName("Id").IsRequired();
        builder.HasQueryFilter(ea => !ea.DeletedAt.HasValue);
    }
}
