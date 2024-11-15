using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Persistence.Contexts;

public class SecurityDbContextFactory : IDesignTimeDbContextFactory<SecurityDbContext>
{
    public SecurityDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SecurityDbContext>();
        optionsBuilder.UseSqlServer(
            "Data Source=localhost,14333;Initial Catalog=dbSecMicroservice;User ID=sa; Password=Abcde@12345;Integrated Security=false;MultipleActiveResultSets=true;TrustServerCertificate=True");

        return new SecurityDbContext(optionsBuilder.Options);
    }
}