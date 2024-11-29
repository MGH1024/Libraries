using MGH.Core.Infrastructure.Securities.Security.Constants;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Security.Infrastructure.Configurations.Base;

namespace Security.Infrastructure.Configurations.Security;

public class OperationClaimConfiguration : IEntityTypeConfiguration<OperationClaim>
{
    public void Configure(EntityTypeBuilder<OperationClaim> builder)
    {
        builder.ToTable(DatabaseTableName.OperationClaim, DatabaseSchema.SecuritySchema).HasKey(ea => ea.Id);
        builder.Property(oc => oc.Id).HasColumnName("Id").IsRequired();
        builder.Property(oc => oc.Name).HasColumnName("Name").IsRequired();
        builder.HasQueryFilter(oc => !oc.DeletedAt.HasValue);
        builder.HasMany(oc => oc.UserOperationClaims);
        builder.HasData(GetSeeds());
    }

    private HashSet<OperationClaim> GetSeeds()
    {
        int id = 0;
        HashSet<OperationClaim> seeds =
            new()
            {
                new OperationClaim { Id = ++id, Name = GeneralOperationClaims.Admin }
            };

        return seeds;
    }
}
