using MGH.Core.Infrastructure.Securities.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Security.Infrastructure.Configurations.Base;

namespace Security.Infrastructure.Configurations.Security;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable(DatabaseTableName.RefreshToken, DatabaseSchema.SecuritySchema).HasKey(ea => ea.Id);

        builder.Property(rt => rt.Id).HasColumnName("Id").IsRequired();
        builder.Property(rt => rt.UserId).IsRequired();
        builder.Property(rt => rt.Token).IsRequired();
        builder.Property(rt => rt.Expires).IsRequired();
        builder.Property(rt => rt.CreatedByIp).IsRequired();
        builder.Property(rt => rt.Revoked);
        builder.Property(rt => rt.RevokedByIp);
        builder.Property(rt => rt.ReplacedByToken);
        builder.Property(rt => rt.ReasonRevoked);

        builder.HasQueryFilter(rt => !rt.DeletedAt.HasValue);

        builder.HasOne(rt => rt.User);
    }
}
