using Library.Domain.Lendings;
using Microsoft.EntityFrameworkCore;
using Library.Domain.Lendings.ValueObjects;
using Library.Infrastructure.Configurations.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Library.Infrastructure.Configurations;

public class LendingEntityTypeConfiguration : IEntityTypeConfiguration<Lending>
{
    public void Configure(EntityTypeBuilder<Lending> builder)
    {
        builder.ToTable(DatabaseTableName.Lending, DatabaseSchema.LibrarySchema);
        
        builder.Property(t => t.Id).IsRequired();
        builder.Property(t => t.BookId).IsRequired();
        builder.Property(t => t.MemberId).IsRequired();
        builder.Property(t => t.LibraryId).IsRequired();
        
     

        var lendingDateConvertor = new ValueConverter<LendingDate, DateTime>(a =>a.Value, a => new LendingDate(a));
        builder.Property(a => a.LendingDate).HasConversion(lendingDateConvertor).IsRequired();
        
        var returnDateConvertor = new ValueConverter<ReturnDate, DateTime>(a =>a.Value, a => new ReturnDate(a));
        builder.Property(a => a.ReturnDate).HasConversion(returnDateConvertor).IsRequired();

        builder.Property(a => a.Version).IsConcurrencyToken();
    }
}