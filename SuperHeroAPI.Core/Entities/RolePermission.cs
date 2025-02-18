using Microsoft.AspNetCore.Identity;

namespace SuperHeroAPI.Core.Entities
{
    public class RolePermission
    {
        public int Id { get; set; }  // Primary Key
        public string RoleId { get; set; }  // Foreign Key to IdentityRole
        public int ModuleId { get; set; }   // Foreign Key to Module

        // Permissions
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
        public bool CanDelete { get; set; }

        // Navigation Properties
        public virtual IdentityRole Role { get; set; }
        public virtual Module Module { get; set; }
        public string PermissionType { get; set; }
    }
}
