using Microsoft.AspNetCore.Identity;
using SuperHeroAPI.Core.Entities;

namespace SuperHeroAPI.Infrastructure.Seed
{
    public static class SeedData
    {
        public static async Task Initialize(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            string[] roleNames = { "SuperAdmin", "Admin", "Manager", "User" };

            // Ensure roles exist
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Seed SuperAdmin User
            string superAdminEmail = "hashim@melior.com";
            var superAdmin = await userManager.FindByEmailAsync(superAdminEmail);

            if (superAdmin == null)
            {
                var newSuperAdmin = new ApplicationUser
                {
                    UserName = "hash", // You can choose a user name
                    Email = superAdminEmail,
                    EmailConfirmed = true,
                    FirstName = "hashim",
                    LastName = "iqbal"

                };

                // Changed the password to include an uppercase letter:
                string password = "Hashim@123"; // Now meets default requirements
                var result = await userManager.CreateAsync(newSuperAdmin, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newSuperAdmin, "SuperAdmin");
                }
                else
                {
                    // Optionally, log errors for debugging:
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception("Seed user creation failed: " + errors);
                }
            }
        }
    }
}
