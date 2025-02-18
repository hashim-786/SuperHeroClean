using Microsoft.AspNetCore.Identity;

namespace SuperHeroAPI.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // A user can have multiple permissions for different modules
        public virtual ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
    }
}
