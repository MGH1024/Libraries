using Domain.Entities.Libraries;
using Domain.Entities.Libraries.Constant;
using Domain.Entities.Libraries.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistence.Configurations.Base;
using District = Domain.Entities.Libraries.ValueObjects.District;

namespace Persistence.Configurations;

public class LibraryConfiguration : IEntityTypeConfiguration<Library>
{
    public void Configure(EntityTypeBuilder<Library> builder)
    {
        //table
        //builder.ToTable(DatabaseTableName.Library, DatabaseSchema.LibrarySchema);


        //fix fields section
        builder.Property(t => t.Id).IsRequired();

        // var libraryNameConvertor =
        //     new ValueConverter<Name, string>
        //         (a => a.Value, a => new Name(a));
        //
        // builder.Property(nameof(Name))
        //     .HasConversion(libraryNameConvertor)
        //     .HasMaxLength(128).IsRequired();
        
        builder.OwnsOne(l => l.Name, name =>
        {
            name.Property(n => n.Value)
                .HasColumnName("Name")
                .IsRequired();
        });

        var libraryCodeConvertor =
            new ValueConverter<Code, string>
                (a => a.Value, a => new Code(a));

        builder.Property(nameof(Code))
            .HasConversion(libraryCodeConvertor)
            .HasMaxLength(3)
            .IsRequired()
            .IsUnicode();

        var libraryLocationConvertor =
            new ValueConverter<Location, string>
                (a => a.Value, a => new Location(a));

        builder.Property(nameof(Location))
            .HasConversion(libraryLocationConvertor)
            .HasMaxLength(256)
            .IsRequired();


        var libraryDistrictConvertor =
            new ValueConverter<District, int>
            (a => (int)a.Value, a =>
                new District((Domain.Entities.Libraries.Constant.District)a));

        builder
            .Property(nameof(District))
            .HasConversion(libraryDistrictConvertor)
            .IsRequired();

        var libraryRegisterDateConvertor =
            new ValueConverter<RegistrationDate, DateTime>
                (a => a.Value, a => new RegistrationDate(a));

        builder
            .Property(nameof(RegistrationDate))
            .HasConversion(libraryRegisterDateConvertor)
            .IsRequired();

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