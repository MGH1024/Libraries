using Library.Infrastructure.Configurations.Base;
using MGH.Core.Infrastructure.Securities.Security.Constants;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Configurations.Security;

public class OperationClaimConfiguration : IEntityTypeConfiguration<OperationClaim>
{
    public void Configure(EntityTypeBuilder<OperationClaim> builder)
    {
        builder.ToTable(DatabaseTableName.OperationClaim, DatabaseSchema.SecuritySchema).HasKey(ea => ea.Id);
        builder.Property(oc => oc.Id).HasColumnName("Id").IsRequired();
        builder.Property(oc => oc.Name).HasColumnName("Name").IsRequired();

        builder.HasQueryFilter(oc => !oc.DeletedAt.HasValue);

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
