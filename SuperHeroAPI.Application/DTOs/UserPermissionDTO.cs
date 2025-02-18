namespace SuperHeroAPI.Application.DTOs
{
    public class UserPermissionDto
    {
        public string UserId { get; set; }  // The ID of the user
        public int ModuleId { get; set; }   // The ID of the module
        public bool CanRead { get; set; }   // Permission for read access
        public bool CanWrite { get; set; }  // Permission for write access
        public bool CanDelete { get; set; } // Permission for delete access
    }
}
