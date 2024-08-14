using MGH.Core.Infrastructure.Securities.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Configurations.Base;

namespace Persistence.Configurations.Security;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshTkn>
{
    public void Configure(EntityTypeBuilder<RefreshTkn> builder)
    {
        builder.ToTable(DatabaseTableName.RefreshToken, DatabaseSchema.SecuritySchema).HasKey(ea => ea.Id);

        builder.Property(rt => rt.Id).HasColumnName("Id").IsRequired();
        builder.Property(rt => rt.UserId).HasColumnName("UserId").IsRequired();
        builder.Property(rt => rt.Token).HasColumnName("Token").IsRequired();
        builder.Property(rt => rt.Expires).HasColumnName("Expires").IsRequired();
        builder.Property(rt => rt.CreatedByIp).HasColumnName("CreatedByIp").IsRequired();
        builder.Property(rt => rt.Revoked).HasColumnName("Revoked");
        builder.Property(rt => rt.RevokedByIp).HasColumnName("RevokedByIp");
        builder.Property(rt => rt.ReplacedByToken).HasColumnName("ReplacedByToken");
        builder.Property(rt => rt.ReasonRevoked).HasColumnName("ReasonRevoked");
        
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
        builder.HasQueryFilter(rt => !rt.DeletedAt.HasValue);

        builder.HasOne(rt => rt.User);
    }
}
