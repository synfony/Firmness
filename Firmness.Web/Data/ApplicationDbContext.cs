using Firmness.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Firmness.Web.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<SaleDetail> SaleDetails { get; set; }
}
