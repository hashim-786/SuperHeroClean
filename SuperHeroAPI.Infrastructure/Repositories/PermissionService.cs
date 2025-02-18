using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SuperHeroAPI.Core.Entities;
using SuperHeroAPI.Core.Interfaces;
using SuperHeroAPI.Infrastructure.Data;

namespace SuperHeroAPI.Infrastructure.Repositories
{
    public class PermissionService : IPermissionService
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public PermissionService(DataContext dataContext, UserManager<ApplicationUser> userManager)
        {
            _dataContext = dataContext;
            _userManager = userManager;
        }

        public async Task<bool> HasPermissionAsync(string userId, int moduleId, string permissionType)
        {
            // First, check if the user exists in the system
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false; // User doesn't exist
            }

            // Fetch the module by ID
            var moduleEntity = await _dataContext.Modules
                .Where(m => m.Id == moduleId)
                .FirstOrDefaultAsync();

            // If module is not found, return false
            if (moduleEntity == null)
            {
                return false; // Module doesn't exist
            }

            // Get the permission from UserPermission table for this specific user and module
            var userPermission = await _dataContext.UserPermissions
                .Where(up => up.UserId == userId && up.ModuleId == moduleId)
                .FirstOrDefaultAsync();

            // If no permission found, return false
            if (userPermission == null)
            {
                return false; // No permissions found for this user in this module
            }

            // Check the permission type (Read, Write, Delete)
            switch (permissionType.ToLower())
            {
                case "read":
                    return userPermission.CanRead;
                case "write":
                    return userPermission.CanWrite;
                case "delete":
                    return userPermission.CanDelete;
                default:
                    return false; // Invalid permission type
            }
        }
    }
}
