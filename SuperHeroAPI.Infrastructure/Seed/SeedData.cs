using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SuperHeroAPI.Core.Entities;
using SuperHeroAPI.Infrastructure.Data;

namespace SuperHeroAPI.Infrastructure.Seed
{
    public static class SeedData
    {
        public static async Task Initialize(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, DataContext context)
        {
            string[] roleNames = { "SuperAdmin", "Admin", "Manager", "User" };

            // Ensure roles exist and get the role ids
            var roleIds = new Dictionary<string, string>();
            foreach (var roleName in roleNames)
            {
                var role = await roleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    var newRole = new IdentityRole(roleName);
                    var createRoleResult = await roleManager.CreateAsync(newRole);
                    if (!createRoleResult.Succeeded)
                    {
                        var errors = string.Join(", ", createRoleResult.Errors.Select(e => e.Description));
                        throw new Exception("Role creation failed: " + errors);
                    }
                    roleIds[roleName] = newRole.Id; // Save the newly created role's Id
                }
                else
                {
                    roleIds[roleName] = role.Id; // Use the existing role's Id
                }
            }

            // Seed Modules (if not already present)
            if (!await context.Modules.AnyAsync())
            {
                var module = new List<Module>
                {
                    new Module { Name = "Users" },
                    new Module { Name = "Orders" },
                    new Module { Name = "Products" }
                };

                await context.Modules.AddRangeAsync(module);
                await context.SaveChangesAsync();
            }

            // Fetch existing users
            var users = await userManager.Users.ToListAsync();
            var modules = await context.Modules.ToListAsync();

            // Iterate through each user and assign permissions based on role
            foreach (var user in users)
            {
                // Get User Roles
                var userRoles = await userManager.GetRolesAsync(user);

                if (userRoles.Contains("SuperAdmin"))
                {
                    // SuperAdmin: Full Permissions
                    foreach (var module in modules)
                    {
                        context.UserPermissions.Add(new UserPermission
                        {
                            UserId = user.Id,
                            ModuleId = module.Id,
                            CanRead = true,
                            CanWrite = true,
                            CanDelete = true
                        });
                    }
                }
                else if (userRoles.Contains("Admin"))
                {
                    // Admin: Can read and write, but not delete
                    foreach (var module in modules)
                    {
                        context.UserPermissions.Add(new UserPermission
                        {
                            UserId = user.Id,
                            ModuleId = module.Id,
                            CanRead = true,
                            CanWrite = true,
                            CanDelete = false
                        });
                    }
                }
                else if (userRoles.Contains("Manager"))
                {
                    // Manager: Can read only
                    foreach (var module in modules)
                    {
                        context.UserPermissions.Add(new UserPermission
                        {
                            UserId = user.Id,
                            ModuleId = module.Id,
                            CanRead = true,
                            CanWrite = false,
                            CanDelete = false
                        });
                    }
                }
                else if (userRoles.Contains("User"))
                {
                    // User: Can read only
                    foreach (var module in modules)
                    {
                        context.UserPermissions.Add(new UserPermission
                        {
                            UserId = user.Id,
                            ModuleId = module.Id,
                            CanRead = true,
                            CanWrite = false,
                            CanDelete = false
                        });
                    }
                }
            }

            // Save all permissions in UserPermission table
            await context.SaveChangesAsync();
        }
    }
}
