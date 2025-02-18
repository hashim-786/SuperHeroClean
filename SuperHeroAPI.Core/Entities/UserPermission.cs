namespace SuperHeroAPI.Core.Entities
{
    public class UserPermission
    {
        public int Id { get; set; }  // Primary Key
        public string UserId { get; set; }  // Foreign Key to ApplicationUser
        public int ModuleId { get; set; }   // Foreign Key to Module

        // Permissions
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
        public bool CanDelete { get; set; }

        // Navigation Properties
        public virtual ApplicationUser User { get; set; }
        public virtual Module Module { get; set; }
    }
}
