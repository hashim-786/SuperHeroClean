using Microsoft.EntityFrameworkCore;
using SuperHeroAPI.Core.Entities;

namespace SuperHeroAPI.Infrastructure.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<SuperHero> SuperHeroes { get; set; }
    }
}
