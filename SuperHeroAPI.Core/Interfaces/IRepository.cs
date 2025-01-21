namespace SuperHeroAPI.Core.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);

        Task AddAsync(T item);
        Task UpdateAsync(T item);
        Task DeleteAsync(int id);

    }
}
