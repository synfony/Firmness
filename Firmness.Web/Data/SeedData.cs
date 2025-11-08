using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System;

namespace Firmness.Web.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // Seed Roles
            string[] roleNames = { "Administrator", "Client" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Seed Administrator User
            var adminUser = await userManager.FindByEmailAsync("admin@example.com");
            if (adminUser == null)
            {
                var newAdminUser = new IdentityUser
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(newAdminUser, "Admin_1234");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdminUser, "Administrator");
                }
            }
        }
    }
}
