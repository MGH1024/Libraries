using MGH.Core.Infrastructure.Securities.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Configurations.Base;

namespace Persistence.Configurations.Security;

public class UserOperationClaimConfiguration : IEntityTypeConfiguration<UserOperationClaim>
{
    public void Configure(EntityTypeBuilder<UserOperationClaim> builder)
    {
        builder.ToTable(DatabaseTableName.UserOperationClaims, DatabaseSchema.SecuritySchema)
            .HasKey(ea => ea.Id);
        
        builder.Property(uoc => uoc.Id).HasColumnName("Id").IsRequired();
        builder.Property(uoc => uoc.UserId).HasColumnName("UserId").IsRequired();
        builder.Property(uoc => uoc.OperationClaimId).HasColumnName("OperationClaimId").IsRequired();
        
        builder.HasOne(uoc => uoc.User);
        builder.HasOne(uoc => uoc.OperationClaim);
        
        
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
            .HasDefaultValue("admin_seed");

        builder.Property(a => a.CreatedAt)
            .HasDefaultValueSql("GetDate()");

        builder.HasData(GetSeeds());
    }

    private IEnumerable<UserOperationClaim> GetSeeds()
    {
        List<UserOperationClaim> userOperationClaims = new();

        UserOperationClaim adminUserOperationClaim = new(id: 1, userId: 1, operationClaimId: 1);
        userOperationClaims.Add(adminUserOperationClaim);

        return userOperationClaims;
    }
}
