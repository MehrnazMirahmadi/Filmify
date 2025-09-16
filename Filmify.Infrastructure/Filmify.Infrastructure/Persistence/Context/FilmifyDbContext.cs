using Filmify.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Filmify.Infrastructure.Persistence.Context;

public class FilmifyDbContext : DbContext
{
    public FilmifyDbContext(DbContextOptions<FilmifyDbContext> options) : base(options)
    {
            
    }

    public DbSet<Film> Films => Set<Film>();
    public DbSet<Box> Boxes => Set<Box>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<FilmBox> FilmBoxes => Set<FilmBox>();
    public DbSet<FilmTag> FilmTags => Set<FilmTag>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FilmifyDbContext).Assembly);
    }
}
