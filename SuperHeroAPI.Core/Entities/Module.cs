namespace SuperHeroAPI.Core.Entities
{
    public class Module
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // A module can have multiple role-based permissions
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

        // A module can have multiple user-specific permissions
        public virtual ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
    }
}
