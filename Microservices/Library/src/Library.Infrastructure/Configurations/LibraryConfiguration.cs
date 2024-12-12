using Library.Infrastructure.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Configurations;

public class LibraryConfiguration : IEntityTypeConfiguration<Domain.Entities.Libraries.Library>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Libraries.Library> builder)
    {
        //table
        builder.ToTable(DatabaseTableName.Library, DatabaseSchema.LibrarySchema);


        //fix fields section
        builder.Property(t => t.Id).IsRequired();
        builder.Property(a => a.Name)
            .HasMaxLength(128).IsRequired();

        builder.Property(a => a.Code)
            .HasMaxLength(3)
            .IsRequired()
            .IsUnicode();

        builder.Property(a => a.Location)
            .HasMaxLength(256)
            .IsRequired();

        builder
            .Property(a => a.District)
            .IsRequired();

        builder
            .Property(a => a.RegistrationDate)
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

        builder.Property(a => a.CreatedAt)
            .HasDefaultValueSql("GetDate()");
    }
}