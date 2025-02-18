using SuperHeroAPI.Core.Entities;

namespace SuperHeroAPI.Core.Interfaces
{
    public interface IModuleRepository
    {
        Task<IEnumerable<Module>> GetAllModulesAsync();
        Task<Module> GetModuleByIdAsync(int id);
        Task<Module> AddModuleAsync(Module module);
    }
}
