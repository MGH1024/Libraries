using Library.Domain.Members;
using Library.Domain.Members.ValueObjects;
using Library.Infrastructure.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Library.Infrastructure.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable(DatabaseTableName.Member, DatabaseSchema.LibrarySchema);
        
        builder.Property(t => t.Id).IsRequired();
        
        var fullNameConvertor = new ValueConverter<FullName, string>(a => a.Value, a => new FullName(a));
        builder.Property(a => a.FullName).HasConversion(fullNameConvertor).HasMaxLength(128).IsRequired();

        var nationalCodeConvertor = new ValueConverter<NationalCode, string>(a => a.Value, a => new NationalCode(a));
        builder.Property(a => a.NationalCode).HasMaxLength(10).IsRequired().HasConversion(nationalCodeConvertor).IsUnicode();
        
        var mobileNumberConvertor = new ValueConverter<MobileNumber, string>(a => a.Value, a => new MobileNumber(a));
        builder.Property(a => a.MobileNumber).HasMaxLength(13).HasConversion(mobileNumberConvertor).IsRequired();
        
        var addressConverter = new ValueConverter<Address, string>(a => a.Value, a => Address.FromValue(a));
        builder.Property(a => a.Address).HasMaxLength(1024).HasConversion(addressConverter).IsRequired();

        builder.Property(a => a.Version).IsConcurrencyToken();
    }
}