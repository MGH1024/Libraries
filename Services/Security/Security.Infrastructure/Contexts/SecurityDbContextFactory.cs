using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Security.Infrastructure.Contexts;

public class SecurityDbContextFactory : IDesignTimeDbContextFactory<SecurityDbContext>
{
    public SecurityDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SecurityDbContext>();
        optionsBuilder.UseSqlServer(
            "Data Source=localhost;Initial Catalog=dbSecMicroservice;User ID=sa; Password=Abcd@1234;Integrated Security=false;MultipleActiveResultSets=true;TrustServerCertificate=True");

        return new SecurityDbContext(optionsBuilder.Options);
    }
}