using System.ComponentModel.DataAnnotations;

namespace SuperHeroAPI.Application.DTOs
{
    public class UpdateUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}
