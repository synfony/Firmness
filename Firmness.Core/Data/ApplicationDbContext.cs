using Firmness.Core.Models;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Firmness.Core.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IDataProtectionKeyContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Admin> Admins { get; set; }
        
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }

        public DbSet<SaleDetail> SaleDetails { get; set; }
        
        // This property is required by IDataProtectionKeyContext
        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Person)
                .WithOne()
                .HasForeignKey<ApplicationUser>(u => u.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure the one-to-one relationship between Client and ApplicationUser
            builder.Entity<Client>()
                .HasOne(c => c.User)
                .WithOne()
                .HasForeignKey<Client>(c => c.UserId)
                .IsRequired(false); // UserId is nullable
        }
    }
}
