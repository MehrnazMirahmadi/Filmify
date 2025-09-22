using Filmify.Domain.Entities;

namespace Filmify.Domain.Contracts.Interfaces;

public interface IFilmRepository : IRepository<Film>
{
    Task<bool> CheckDuplicateTitleAsync(string title);
    Task<Film?> GetFilmWithRelationsAsync(long id);
    IQueryable<Film> QueryWithRelations();
    //Task<IEnumerable<Film>> SearchAsync(string? key, int page = 1, int pageSize = 10);
    Task<IEnumerable<Film>> SearchAsync(string? key, long? lastKey, int pageSize = 10);
    Task<int> CountAsync(string? key);
}
