using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Library.Infrastructure.Contexts;

public class SecurityDbContextFactory : IDesignTimeDbContextFactory<PublicLibraryDbContext>
{
    public PublicLibraryDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PublicLibraryDbContext>();
        optionsBuilder.UseSqlServer(
            "Data Source=localhost;Initial Catalog=dbLibraryMicroservice;User ID=sa; Password=Abcd@1234;Integrated Security=false;MultipleActiveResultSets=true;TrustServerCertificate=True");

        return new PublicLibraryDbContext(optionsBuilder.Options);
    }
}