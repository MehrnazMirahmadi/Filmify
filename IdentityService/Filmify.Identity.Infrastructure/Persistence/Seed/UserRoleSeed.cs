using Filmify.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Filmify.Identity.Infrastructure.Persistence.Seed;

public static class UserRoleSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity(j => j.HasData(
                new { RolesId = 1L, UsersId = 1L }, // Admin → Mehrnaz
                new { RolesId = 2L, UsersId = 2L }  // User → Sahar
            ));
    }
}