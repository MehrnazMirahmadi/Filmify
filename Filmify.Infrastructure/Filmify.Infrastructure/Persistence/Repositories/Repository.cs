using Filmify.Domain.Contracts.Interfaces;
using Filmify.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Filmify.Infrastructure.Persistence.Repositories;

public class Repository<T>(FilmifyDbContext filmifyDb) : IRepository<T> where T : class
{
    // DbSet مستقیماً از constructor init می‌شود
    protected readonly DbSet<T> _dbSet = filmifyDb.Set<T>();

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public virtual async Task<T> GetByIdAsync(long id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public virtual Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public virtual async Task DeleteAsync(long id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
            _dbSet.Remove(entity);
    }
    public virtual IQueryable<T> Query()
    {
        return _dbSet.AsQueryable();
    }
}
