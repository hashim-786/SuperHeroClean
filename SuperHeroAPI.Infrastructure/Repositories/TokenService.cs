using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SuperHeroAPI.Application.Interfaces;
using SuperHeroAPI.Core.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SuperHeroAPI.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;  // Inject UserManager

        public TokenService(IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<string> GenerateToken(ApplicationUser user)
        {
            // 1. Get roles for the user
            var roles = await _userManager.GetRolesAsync(user);

            // 2. Define claims (User info + roles in the token)
            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            // 3. Add roles and roleId as claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));


                // Get the roleId from the database (assuming you have a Role entity with Id and Name)
                var roleEntity = await _userManager.FindByNameAsync(role);
                Console.WriteLine(roleEntity);
                if (roleEntity != null)
                {
                    claims.Add(new Claim("roleId", roleEntity.Id));  // Add the roleId as a custom claim
                }
            }

            // 4. Get the secret key from appsettings.json
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            // 5. Create signing credentials
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 6. Define token properties
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1), // Token valid for 1 hour
                SigningCredentials = credentials,
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            // 7. Create the JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // 8. Return the token string
            return tokenHandler.WriteToken(token);
        }

    }
}
