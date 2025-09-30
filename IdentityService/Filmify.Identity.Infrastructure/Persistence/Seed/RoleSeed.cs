using Filmify.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Filmify.Identity.Infrastructure.Persistence.Seed;

public class RoleSeed : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasData(
            new Role { Id = 1, Name = "Admin", Description = "System Administrator" },
            new Role { Id = 2, Name = "User", Description = "Regular User" }
        );
    }
}