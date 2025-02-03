using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SuperHeroAPI.Application.DTOs; // For UpdateUserDto and (if needed) RegisterDto
using SuperHeroAPI.Core.Constants;    // For UserRoles constants
using SuperHeroAPI.Core.Entities;     // For ApplicationUser

namespace SuperHeroAPI.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = UserRoles.SuperAdmin)]
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] SuperHeroAPI.Application.DTOs.RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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
        [Authorize(Roles = $"{UserRoles.SuperAdmin},{UserRoles.Admin},{UserRoles.Manager},{UserRoles.User}")]
        [HttpGet("all")]
        public IActionResult GetAllUsers()
        {
            // Return a list of users with some key properties.
            var users = _userManager.Users.Select(u => new
            {
                u.Id,
                u.Email,
                u.FirstName,
                u.LastName
            }).ToList();

            return Ok(users);
        }

        // PUT: api/users/update/{id}
        // SuperAdmin, Admin, and Manager can update users.
        [Authorize(Roles = $"{UserRoles.SuperAdmin},{UserRoles.Admin},{UserRoles.Manager}")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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
        [Authorize(Roles = $"{UserRoles.SuperAdmin},{UserRoles.Admin}")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
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
