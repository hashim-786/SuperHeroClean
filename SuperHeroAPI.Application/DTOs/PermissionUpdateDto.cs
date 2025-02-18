namespace SuperHeroAPI.Application.DTOs
{
    public class PermissionUpdateDto
    {
        public string RoleId { get; set; }       // The ID of the role (Admin, Manager, etc.)
        public int ModuleId { get; set; }        // The ID of the module (e.g., "Users", "Projects")
        public bool CanRead { get; set; }        // Indicates if the role has read permission
        public bool CanWrite { get; set; }       // Indicates if the role has write permission
        public bool CanDelete { get; set; }      // Indicates if the role has delete permission
        public string PermissionType { get; set; } // The permission type (e.g., "Admin", "User")
    }
}
