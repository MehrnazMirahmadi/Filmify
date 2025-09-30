using Filmify.Identity.Infrastructure.Identity;
using Filmify.Identity.Infrastructure.Persistence.Seed;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Filmify.Identity.Infrastructure.Persistence.Context;

public class FilmifyIdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, long>
{
    public FilmifyIdentityDbContext(DbContextOptions<FilmifyIdentityDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // apply seeds
        builder.ApplyConfiguration(new RoleSeed());
        builder.ApplyConfiguration(new UserSeed());
        UserRoleSeed.Seed(builder);
    }
}
