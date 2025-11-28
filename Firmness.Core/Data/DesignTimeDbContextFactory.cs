using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Firmness.Core.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // This is a bit of a hack to get the appsettings.json file from the Web project
            // It's necessary because the DbContext is in a different project
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Firmness.Web"))
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetConnectionString("Default");
            builder.UseNpgsql(connectionString, 
                o => o.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));

            return new ApplicationDbContext(builder.Options);
        }
    }
}
