using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Green.Infrastructure;

public class DesignTimeGreenDbContextFactory : IDesignTimeDbContextFactory<GreenDbContext>
{
    public GreenDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<GreenDbContext>();
        optionsBuilder.UseSqlite("DataSource=../Green.Infrastructure/Green.db");

        return new GreenDbContext(optionsBuilder.Options);
    }
}
