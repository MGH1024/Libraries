using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Infrastructure.Securities.Security.Hashing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Security.Infrastructure.Configurations.Base;

namespace Security.Infrastructure.Configurations.Security;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(DatabaseTableName.User, DatabaseSchema.SecuritySchema)
            .HasKey(ea => ea.Id);
        builder.Property(u => u.Id).HasColumnName("Id").IsRequired();
        builder.Property(u => u.FirstName).IsRequired();
        builder.Property(u => u.LastName).IsRequired();
        builder.Property(u => u.Email).IsRequired();
        builder.Property(u => u.PasswordSalt).IsRequired();
        builder.Property(u => u.PasswordHash).IsRequired();

        builder.HasQueryFilter(u => !u.DeletedAt.HasValue);

        builder.HasMany(u => u.UserOperationClaims);
        builder.HasMany(u => u.RefreshTokens);
        
        builder.HasData(GetSeeds());
    }

    private IEnumerable<User> GetSeeds()
    {
        List<User> users = new();

        var hashingHelperModel =  HashingHelper.CreatePasswordHash(password: "Abcd@1234");
        User adminUser =
            new()
            {
                Id = 1,
                FirstName = "Admin",
                LastName = "Admin",
                Email = "admin@admin.com",
                PasswordHash = hashingHelperModel.PasswordHash,
                PasswordSalt = hashingHelperModel.PasswordSalt
            };
        users.Add(adminUser);

        return users.ToArray();
    }
}
