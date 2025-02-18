namespace SuperHeroAPI.Core.Interfaces
{
    public interface IPermissionService
    {
        Task<bool> HasPermissionAsync(string userId, int module, string permissionType);
    }
}

