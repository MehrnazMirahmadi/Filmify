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
    // ریپوزیتوری (infra)
    //public async Task<IEnumerable<Film>> SearchAsync(string? key, int page = 1, int pageSize = 10)
    //{
    //    if (page < 1) page = 1;

    //    var query = _dbSet
    //        .Include(f => f.Category)
    //        .Include(f => f.FilmTags).ThenInclude(ft => ft.Tag)
    //        .AsQueryable();

    //    if (!string.IsNullOrEmpty(key))
    //    {
    //        query = query.Where(f =>
    //            f.FilmTitle.Contains(key) ||
    //            f.FilmTags.Any(ft => ft.Tag.TagText.Contains(key)) ||
    //            f.Category.Name.Contains(key)
    //        );
    //    }

    //    query = query.OrderBy(f => f.FilmTitle);

    //    return await query
    //        .Skip((page - 1) * pageSize)
    //        .Take(pageSize)
    //        .AsNoTracking()
    //        .ToListAsync();
    //}
    public async Task<IEnumerable<Film>> SearchAsync(string? key, long? lastKey, int pageSize = 10)
    {
        var query = _dbSet
            .Include(f => f.Category)
            .Include(f => f.FilmTags).ThenInclude(ft => ft.Tag)
            .AsQueryable();

        if (!string.IsNullOrEmpty(key))
        {
            query = query.Where(f =>
                f.FilmTitle.Contains(key) ||
                f.FilmTags.Any(ft => ft.Tag.TagText.Contains(key)) ||
                f.Category.Name.Contains(key)
            );
        }

        query = query.OrderBy(f => f.FilmId);

        
        if (lastKey.HasValue && lastKey.Value > 0)
        {
            query = query.Where(f => f.FilmId > lastKey.Value);
        }

        return await query
            .Take(pageSize + 1) 
            .AsNoTracking()
            .ToListAsync();
    }



    public async Task<Film?> GetFilmWithRelationsAsync(long id)
    {
        return await db.Films
            .Include(f => f.FilmTags).ThenInclude(ft => ft.Tag)
            .Include(f => f.FilmBoxes).ThenInclude(fb => fb.Box)
            .FirstOrDefaultAsync(f => f.FilmId == id);
    }
    // ✅ QueryWithRelations method
    public IQueryable<Film> QueryWithRelations()
    {
        return db.Films
       .Include(f => f.Category)
       .Include(f => f.FilmBoxes).ThenInclude(fb => fb.Box)
       .Include(f => f.FilmTags).ThenInclude(ft => ft.Tag);
    }

    public async Task<int> CountAsync(string? key)
    {
        var query = QueryWithRelations();

        if (!string.IsNullOrEmpty(key))
        {
            query = query.Where(f =>
                f.FilmTitle.Contains(key) ||
                f.FilmTags.Any(ft => ft.Tag.TagText.Contains(key)) ||
                f.Category.Name.Contains(key));
        }

        return await query.CountAsync();
    }

}