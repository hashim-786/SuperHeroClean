using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SuperHeroAPI.Application.DTOs; // For UpdateUserDto and (if needed) RegisterDto
using SuperHeroAPI.Core.Entities;
using SuperHeroAPI.Core.Interfaces;
using SuperHeroAPI.Infrastructure.Data;

namespace SuperHeroAPI.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPermissionService _permissionService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(DataContext context, UserManager<ApplicationUser> userManager, IPermissionService permissionService, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _permissionService = permissionService;
            _roleManager = roleManager;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] SuperHeroAPI.Application.DTOs.RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userId = _userManager.GetUserId(User); // Get logged-in user's ID

            var hasPermission = await _permissionService.HasPermissionAsync(userId, 1, "Write");

            if (!hasPermission)
                return Forbid();

            // Check if a user with the given email already exists.
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "User with this email already exists." });
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Assign role to the new user.
            // model.Role should be provided in the request (e.g., "Admin", "Manager", "User")
            if (!await _roleManager.RoleExistsAsync(model.Role))
            {
                return BadRequest(new { message = "Specified role does not exist." });
            }
            await _userManager.AddToRoleAsync(user, model.Role);

            return Ok(new { message = $"User created successfully with role {model.Role}.", userId = user.Id });
        }

        // GET: api/users/all
        // All roles (SuperAdmin, Admin, Manager, and User) can view users.

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {

            // Permission Check Logic
            var userID = _userManager.GetUserId(User);
            Console.WriteLine(userID);

            var hasPermission = await _permissionService.HasPermissionAsync(userID, 1, "Read");
            Console.WriteLine(hasPermission);

            if (!hasPermission)
                return Forbid();

            if (!hasPermission) { return Forbid(); }


            var users = _userManager.Users.ToList();

            var userList = new List<object>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new
                {
                    user.Id,
                    user.Email,
                    user.FirstName,
                    user.LastName,
                    Role = roles.FirstOrDefault() // Get first role if exists
                });
            }

            return Ok(userList);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var userID = _userManager.GetUserId(User);


            var hasPermission = await _permissionService.HasPermissionAsync(userID, 1, "Read");

            if (!hasPermission)
                return Forbid();
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            var userDto = new
            {
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName
            };

            return Ok(userDto);
        }


        // PUT: api/users/update/{id}
        // SuperAdmin, Admin, and Manager can update users.

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            // Permission Check Logic
            var userID = _userManager.GetUserId(User);



            var hasPermission = await _permissionService.HasPermissionAsync(userID, 1, "Write");

            if (!hasPermission)
                return Forbid();

            if (!hasPermission) { return Forbid(); }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }



            // Update the user properties.
            user.Email = updateDto.Email;
            user.UserName = updateDto.Email; // Optionally update username if it should match email.
            user.FirstName = updateDto.FirstName;
            user.LastName = updateDto.LastName;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { message = $"User {id} updated successfully." });
        }

        // DELETE: api/users/delete/{id}
        // Only SuperAdmin and Admin can delete users.

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {

            // Permission Check Logic
            var userID = _userManager.GetUserId(User);
            var hasPermission = await _permissionService.HasPermissionAsync(userID, 2, "Delete");

            if (!hasPermission) { return Forbid(); }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { message = $"User {id} deleted successfully." });
        }
    }
}
