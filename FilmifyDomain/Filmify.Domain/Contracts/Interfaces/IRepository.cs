namespace Filmify.Domain.Contracts.Interfaces;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(long id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(long id);
    IQueryable<T> Query();
}
