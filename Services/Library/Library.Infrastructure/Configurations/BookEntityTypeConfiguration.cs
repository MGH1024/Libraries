using Library.Domain.Books;
using Microsoft.EntityFrameworkCore;
using Library.Domain.Books.ValueObjects;
using Library.Infrastructure.Configurations.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Library.Infrastructure.Configurations;

public class BookEntityTypeConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable(DatabaseTableName.Book, DatabaseSchema.LibrarySchema);
        
        builder.Property(t => t.Id).IsRequired();
        
        var isbnConvertor = new ValueConverter<Isbn, string>(a => a.Value, a => new Isbn(a));
        builder.Property(a => a.Isbn).HasConversion(isbnConvertor).HasMaxLength(13).IsRequired();

        var titleConvertor = new ValueConverter<Title, string>(a => a.Value, a => new Title(a));
        builder.Property(a => a.Title).HasMaxLength(256).IsRequired().HasConversion(titleConvertor).IsUnicode();
        
        var uniqueCodeConvertor = new ValueConverter<UniqueCode, string>(a => a.Value, a => new UniqueCode(a));
        builder.Property(a => a.UniqueCode).HasMaxLength(32).HasConversion(uniqueCodeConvertor).IsRequired();
        
        var isReferenceConvertor = new ValueConverter<IsReference, bool>(a => a.Value, a => new IsReference(a));
        builder.Property(a => a.IsReference).HasConversion(isReferenceConvertor).IsRequired();

        var publicationDateConvertor = new ValueConverter<PublicationDate, DateTime>(a =>a.Value, a => new PublicationDate(a));
        builder.Property(a => a.PublicationDate).HasConversion(publicationDateConvertor).IsRequired();

        builder.OwnsMany(
            cu => cu.BookAuthors,
            a =>
            {
                a.ToTable(DatabaseTableName.Author);
                a.WithOwner().HasForeignKey("BookId"); // Shadow Foreign Key
                a.Property<Guid>("Id"); // Shadow property
                a.HasKey("Id"); // Shadow Primary Key
                a.Property(x => x.FullName).HasMaxLength(128).IsRequired();
                a.Property(x => x.NationalCode).IsRequired().HasMaxLength(11);
            });

        builder.Property(a=>a.Version).IsConcurrencyToken();
        
    }
}