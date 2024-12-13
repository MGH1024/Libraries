using Library.Domain.Entities.Libraries.Constant;
using Library.Domain.Entities.Libraries.ValueObjects;
using Library.Infrastructure.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Library.Infrastructure.Configurations;

public class LibraryConfiguration : IEntityTypeConfiguration<Domain.Entities.Libraries.Library>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Libraries.Library> builder)
    {
        builder.ToTable(DatabaseTableName.Library, DatabaseSchema.LibrarySchema);
        
        builder.Property(t => t.Id).IsRequired();
        
        var nameConvertor = new ValueConverter<Name, string>(a => a.Value, a => new Name(a));
        builder.Property(a => a.Name).HasConversion(nameConvertor).HasMaxLength(128).IsRequired();

        var codeConvertor = new ValueConverter<Code, string>(a => a.Value, a => new Code(a));
        builder.Property(a => a.Code).HasMaxLength(3).IsRequired().HasConversion(codeConvertor).IsUnicode();
        
        var locationConvertor = new ValueConverter<Location, string>(a => a.Value, a => new Location(a));
        builder.Property(a => a.Location).HasMaxLength(256).HasConversion(locationConvertor).IsRequired();
        
        var districtConvertor = new ValueConverter<District, int>(a => (int)a.Value, a => new District((DistrictEnum)a));
        builder.Property(a => a.District).HasConversion(districtConvertor).IsRequired();

        var registrationDateConvertor = new ValueConverter<RegistrationDate, DateTime>(a =>a.Value, a => new RegistrationDate(a));
        builder.Property(a => a.RegistrationDate).HasConversion(registrationDateConvertor).IsRequired();

        builder.OwnsMany(
            cu => cu.LibraryStaves,
            a =>
            {
                a.ToTable(DatabaseTableName.Staff);
                a.WithOwner().HasForeignKey("LibraryId"); // Shadow Foreign Key
                a.Property<Guid>("Id"); // Shadow property
                a.HasKey("Id"); // Shadow Primary Key
                a.Property(x => x.Name).IsRequired();
                a.Property(x => x.Position).IsRequired().HasMaxLength(64);
                a.Property(x => x.NationalCode).IsRequired().HasMaxLength(30);
            });
        
    }
}