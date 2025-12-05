using Firmness.Core.Data;
using Firmness.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Firmness.Web.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            await context.Database.EnsureCreatedAsync();

            string[] roleNames = { "Administrator", "Client" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var adminEmail = "admin@firmness.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminPerson = new Admin
                {
                    FirstName = "Admin",
                    LastName = "User",
                    DocumentId = "00000000",
                    Address = "Admin Address",
                    PhoneNumber = "0000000000",
                    PersonType = "Admin",
                    SpecialRole = "SuperAdmin"
                };
                context.Admins.Add(adminPerson);
                await context.SaveChangesAsync();

                var newAdminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    PersonId = adminPerson.Id,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(newAdminUser, "Admin123*");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdminUser, "Administrator");
                }
            }

            var clientEmail = "cliente@firmness.com";
            if (await userManager.FindByEmailAsync(clientEmail) == null)
            {
                var clientPerson = new Client
                {
                    FirstName = "Client",
                    LastName = "User",
                    DocumentId = "11111111",
                    Address = "Client Address",
                    PhoneNumber = "1111111111",
                    PersonType = "Client",
                    PurchaseHistory = "Initial purchase"
                };
                context.Clients.Add(clientPerson);
                await context.SaveChangesAsync();

                var newClientUser = new ApplicationUser
                {
                    UserName = clientEmail,
                    Email = clientEmail,
                    PersonId = clientPerson.Id,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(newClientUser, "Client123*");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newClientUser, "Client");
                }
            }
        }
    }
}
