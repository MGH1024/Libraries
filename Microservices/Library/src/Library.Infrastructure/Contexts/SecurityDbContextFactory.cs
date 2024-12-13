using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Library.Infrastructure.Contexts;

public class SecurityDbContextFactory : IDesignTimeDbContextFactory<LibraryDbContext>
{
    public LibraryDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<LibraryDbContext>();
        optionsBuilder.UseSqlServer(
            "Data Source=localhost;Initial Catalog=dbLibraryMicroservice;User ID=sa; Password=Abcd@1234;Integrated Security=false;MultipleActiveResultSets=true;TrustServerCertificate=True");

        return new LibraryDbContext(optionsBuilder.Options);
    }
}