using Filmify.Domain.Contracts.Interfaces;
using Filmify.Domain.Entities;
using Filmify.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Filmify.Infrastructure.Persistence.Repositories;

public class FilmRepository(FilmifyDbContext db) : Repository<Film>(db), IFilmRepository
{
    public async Task<bool> CheckDuplicateTitleAsync(string title)
    {
        return await _dbSet.AnyAsync(f => f.FilmTitle == title);
    }

    public async Task<IEnumerable<Film>> SearchAsync(string? key, int page = 1, int pageSize = 10)
    {
        var query = _dbSet.AsQueryable();

        if (!string.IsNullOrEmpty(key))
            query = query.Where(f => f.FilmTitle.Contains(key));

        query = query.OrderBy(f => f.FilmTitle);

        return await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }
}