using Filmify.Domain.Entities;

namespace Filmify.Domain.Contracts.Interfaces;

public interface IFilmRepository : IRepository<Film>
{
    Task<bool> CheckDuplicateTitleAsync(string title);
}
