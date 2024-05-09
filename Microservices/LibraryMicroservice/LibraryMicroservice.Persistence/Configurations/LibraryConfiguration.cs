using Domain.Entities.Libraries;
using Domain.Entities.Libraries.Constant;
using Domain.Entities.Libraries.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistence.Configurations.Base;

namespace Persistence.Configurations;

public class LibraryConfiguration : IEntityTypeConfiguration<Library>
{
    public void Configure(EntityTypeBuilder<Library> builder)
    {
        //table
        //builder.ToTable(DatabaseTableName.Library, DatabaseSchema.LibrarySchema);


        //fix fields section
        builder.Property(t => t.Id).IsRequired();

        var libraryNameConvertor = new ValueConverter<LibraryName, string>(a => a.Value, a => new LibraryName(a));
        builder.Property(typeof(LibraryName), "LibraryName").HasConversion(libraryNameConvertor).HasColumnName("Name")
            .HasMaxLength(128).IsRequired();
        
        var libraryCodeConvertor = new ValueConverter<LibraryCode, string>(a => a.Value, a => new LibraryCode(a));
        builder.Property(typeof(LibraryCode), "LibraryCode").HasConversion(libraryCodeConvertor).HasColumnName("Code")
            .HasMaxLength(3).IsRequired().IsUnicode();
        
        var libraryLocationConvertor = 
            new ValueConverter<LibraryLocation, string>(a => a.Value, a => new LibraryLocation(a));
        builder.Property(typeof(LibraryLocation), "LibraryLocation").HasConversion(libraryLocationConvertor)
            .HasColumnName("Location").HasMaxLength(256).IsRequired();
        
        
        var libraryDistrictConvertor = 
            new ValueConverter<LibraryDistrict, int>(a => (int)a.Value, a => new LibraryDistrict((District)a));
        builder.Property(typeof(LibraryDistrict), "LibraryDistrict").HasConversion(libraryDistrictConvertor)
            .HasColumnName("District").IsRequired();
        
        var libraryRegisterDateConvertor = 
            new ValueConverter<LibraryRegistrationDate, DateTime>(a => a.Value, a => new LibraryRegistrationDate(a));
        builder.Property(typeof(LibraryRegistrationDate), "LibraryRegistrationDate")
            .HasConversion(libraryRegisterDateConvertor).HasColumnName("RegistrationDate").IsRequired();

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


        // //public
        // builder.Ignore(a => a.Row);
        // builder.Ignore(a => a.PageSize);
        // builder.Ignore(a => a.TotalCount);
        // builder.Ignore(a => a.CurrentPage);
        //
        // builder.Ignore(a => a.ListItemText);
        // builder.Ignore(a => a.ListItemTextForAdmins);

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

        builder.Property(a => a.CreatedAt);
        //.HasDefaultValueSql("GetDate()")
        //.HasDefaultValueSql("now()");
    }
}