using Microsoft.EntityFrameworkCore;

namespace Filmify.Infrastructure.Persistence.Context;

public class FilmifyDbContext : DbContext
{
    public FilmifyDbContext(DbContextOptions<FilmifyDbContext> options) : base(options)
    {
            
    }
}
