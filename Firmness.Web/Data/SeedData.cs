using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Firmness.Web.Models;

namespace Firmness.Web.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            // Ensure database exists
            await context.Database.EnsureCreatedAsync();

            // ---- Seed Roles ----
            string[] roles = { "Administrator", "Client" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // ---- Seed Admin Person ----
            var adminPerson = new Admin
            {
                FirstName = "System",
                LastName = "Administrator",
                DocumentId = "0000",
                Address = "N/A",
                PhoneNumber = "0000000000",
                PersonType = "Admin",
                SpecialRole = "SuperAdmin"
            };

            // Add Person only if does not exist
            if (context.Admins.Any() == false)
            {
                context.Admins.Add(adminPerson);
                await context.SaveChangesAsync();
            }

            // ---- Seed Admin User ----
            string adminEmail = "admin@example.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    PersonId = adminPerson.Id
                };

                var result = await userManager.CreateAsync(adminUser, "Admin_1234");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Administrator");
                }
            }
        }
    }
}


