using Filmify.Domain.Entities;

namespace Filmify.Domain.Contracts.Interfaces;

public interface IRepository<T> where T : class
{
    IQueryable<T> GetAll();
    Task<T> GetByIdAsync(long id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(long id);
    IQueryable<T> Query();
    
}
