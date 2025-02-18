using Microsoft.EntityFrameworkCore;
using SuperHeroAPI.Core.Entities;
using SuperHeroAPI.Core.Interfaces;
using SuperHeroAPI.Infrastructure.Data;

namespace SuperHeroAPI.Infrastructure.Repositories
{
    public class ModuleRepository : IModuleRepository
    {
        private readonly DataContext _context;

        public ModuleRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Module>> GetAllModulesAsync()
        {
            return await _context.Modules.ToListAsync();
        }

        public async Task<Module> GetModuleByIdAsync(int id)
        {
            return await _context.Modules.FindAsync(id);
        }

        public async Task<Module> AddModuleAsync(Module module)
        {
            _context.Modules.Add(module);
            await _context.SaveChangesAsync();
            return module;
        }
    }
}
