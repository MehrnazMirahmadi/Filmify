using Filmify.Identity.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Filmify.Identity.Infrastructure.Persistence.Context;

public class FilmifyIdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, long>
{
    public FilmifyIdentityDbContext(DbContextOptions<FilmifyIdentityDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

    }
}
