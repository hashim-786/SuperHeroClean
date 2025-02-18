using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperHeroAPI.Application.DTOs;
using SuperHeroAPI.Core.Constants;
using SuperHeroAPI.Core.Entities;
using SuperHeroAPI.Core.Interfaces;
using SuperHeroAPI.Infrastructure.Data;

[Authorize(Roles = $"{UserRoles.SuperAdmin}")]
[ApiController]
[Route("api/permissions")]
public class PermissionController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IPermissionService _permissionService;
    private readonly UserManager<ApplicationUser> _userManager;

    public PermissionController(DataContext context, IPermissionService permissionService, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _permissionService = permissionService;
        _userManager = userManager;
    }

    // POST: api/permissions/update-permissions
    [HttpPost("update-permissions")]
    public async Task<IActionResult> UpdatePermissions([FromBody] List<UserPermissionDto> permissionUpdates)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Retrieve all claims from the JWT token
        var userID = _userManager.GetUserId(User);

        // Check if the user has permission for the given moduleId
        // Assume role-based access for writing permission in a module
        var hasPermission = await _permissionService.HasPermissionAsync(userID, permissionUpdates.First().ModuleId, "Write");

        if (!hasPermission)
            return Forbid();

        // Iterate through the permission updates and process them
        foreach (var update in permissionUpdates)
        {
            var rolePermission = await _context.UserPermissions
                .FirstOrDefaultAsync(rp => rp.UserId == update.UserId && rp.ModuleId == update.ModuleId);

            if (rolePermission != null)
            {
                // Update existing permissions
                rolePermission.CanRead = update.CanRead;
                rolePermission.CanWrite = update.CanWrite;
                rolePermission.CanDelete = update.CanDelete;

            }
            else
            {
                // Create new role permission if it doesn't exist
                var newRolePermission = new RolePermission
                {
                    RoleId = update.UserId,
                    ModuleId = update.ModuleId,
                    CanRead = update.CanRead,
                    CanWrite = update.CanWrite,
                    CanDelete = update.CanDelete,

                };

                _context.RolePermissions.Add(newRolePermission);
            }
        }

        // Save the changes to the database
        await _context.SaveChangesAsync();

        return Ok(new { message = "Permissions updated successfully." });
    }

    // GET: api/permissions
    [HttpGet]
    public async Task<IActionResult> GetPermissions()
    {
        var permissions = await _context.UserPermissions
         .Include(up => up.User) // Include the ApplicationUser to get email
         .Select(up => new
         {
             up.UserId,
             UserEmail = up.User.Email, // Get the user's email
             up.ModuleId,
             up.CanRead,
             up.CanWrite,
             up.CanDelete
         })
         .ToListAsync();

        return Ok(permissions);
    }

    // GET: api/permissions/{roleId}/{moduleId}
    [HttpGet("{userId}/{moduleId}")]
    public async Task<IActionResult> GetPermissionsByUserAndModule(string userId, int moduleId)
    {
        // Retrieve permissions for the given user and module
        var userPermission = await _context.UserPermissions
            .FirstOrDefaultAsync(up => up.UserId == userId && up.ModuleId == moduleId);

        if (userPermission == null)
        {
            return NotFound(new { message = "No permission found for this user and module." });
        }

        return Ok(new
        {
            userPermission.UserId,
            userPermission.ModuleId,
            userPermission.CanRead,
            userPermission.CanWrite,
            userPermission.CanDelete,

        });
    }

    // GET: api/permissions/users-by-module/{moduleId}
    [Authorize(Roles = $"{UserRoles.SuperAdmin}")]
    [HttpGet("users-by-module/{moduleId}")]
    public async Task<IActionResult> GetUsersByModule(int moduleId)
    {
        // Join UserPermissions with ApplicationUser to get the email, and filter by ModuleId
        var permissions = await _context.UserPermissions
            .Where(up => up.ModuleId == moduleId) // Filter by moduleId
            .Include(up => up.User) // Include the ApplicationUser to get email
            .Select(up => new
            {
                up.UserId,
                UserEmail = up.User.Email, // Get the user's email
                up.ModuleId,
                up.CanRead,
                up.CanWrite,
                up.CanDelete
            })
            .ToListAsync();

        if (permissions == null || !permissions.Any())
        {
            return NotFound(new { message = "No users found for the specified module." });
        }

        return Ok(permissions);
    }


}
