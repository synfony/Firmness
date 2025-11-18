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

            if (!context.Admins.Any())
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

            // ---- Seed Client Person ----
            var clientPerson = new Client
            {
                FirstName = "Test",
                LastName = "User",
                DocumentId = "1111",
                Address = "Client Street",
                PhoneNumber = "1234567890",
                PersonType = "Client"
            };

            if (!context.Clients.Any())
            {
                context.Clients.Add(clientPerson);
                await context.SaveChangesAsync();
            }

            // ---- Seed Client User ----
            string clientEmail = "client@example.com";
            var clientUser = await userManager.FindByEmailAsync(clientEmail);

            if (clientUser == null)
            {
                clientUser = new ApplicationUser
                {
                    UserName = clientEmail,
                    Email = clientEmail,
                    EmailConfirmed = true,
                    PersonId = clientPerson.Id
                };

                var result = await userManager.CreateAsync(clientUser, "Client_1234");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(clientUser, "Client");
                }
            }
        }
    }
}
