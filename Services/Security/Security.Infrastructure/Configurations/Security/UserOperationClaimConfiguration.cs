using MGH.Core.Infrastructure.Securities.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Security.Infrastructure.Configurations.Base;

namespace Security.Infrastructure.Configurations.Security;

public class UserOperationClaimConfiguration : IEntityTypeConfiguration<UserOperationClaim>
{
    public void Configure(EntityTypeBuilder<UserOperationClaim> builder)
    {
        builder.ToTable(DatabaseTableName.UserOperationClaims, DatabaseSchema.SecuritySchema)
            .HasKey(ea => ea.Id);
        
        builder.Property(uoc => uoc.Id).IsRequired();
        builder.Property(uoc => uoc.UserId).IsRequired();
        builder.Property(uoc => uoc.OperationClaimId).IsRequired();
        
        builder.HasOne(uoc => uoc.User);
        builder.HasOne(uoc => uoc.OperationClaim);

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
