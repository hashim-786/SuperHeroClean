using Microsoft.EntityFrameworkCore;
using SuperHeroAPI.Core.Entities;
using SuperHeroAPI.Core.Interfaces;
using SuperHeroAPI.Infrastructure.Data;

namespace SuperHeroAPI.Infrastructure.Repositories
{
    public class SuperHeroRepository : ISuperHeroRepository
    {
        private readonly DataContext _dataContext;

        public SuperHeroRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<SuperHero>> GetAllAsync()
        {
            return await _dataContext.SuperHeroes.ToListAsync();
        }
        public async Task<SuperHero> GetByIdAsync(int id)
        {
            return await _dataContext.SuperHeroes.FindAsync(id);
        }

        public async Task AddAsync(SuperHero hero)
        {
            await _dataContext.SuperHeroes.AddAsync(hero);
            _dataContext.SaveChanges();

        }

        public async Task UpdateAsync(SuperHero hero)
        {
            _dataContext.SuperHeroes.Update(hero);
            await _dataContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var hero = await _dataContext.SuperHeroes.FirstOrDefaultAsync(x => x.Id == id);
            if (hero != null)
            {
                _dataContext.SuperHeroes.Remove(hero);
                _dataContext.SaveChanges();

            }
        }


    }
}
