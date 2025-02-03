using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SuperHeroAPI.Application.DTOs;
using SuperHeroAPI.Application.Interfaces;
using SuperHeroAPI.Core.Entities;

namespace SuperHeroAPI.Web.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;

        public AuthController(UserManager<ApplicationUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("User registered successfully!");
        }


        // POST: api/auth/login
        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            Console.WriteLine($"Login attempt: {model.Email}"); // Debug log

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                Console.WriteLine("Invalid credentials");
                return Unauthorized(new { Message = "Invalid credentials" });
            }

            var token = await _tokenService.GenerateToken(user);
            return Ok(new { Token = token });
        }


    }
}
