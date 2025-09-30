using Filmify.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Filmify.Identity.Infrastructure.Persistence.Seed;

public class UserSeed : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasData(
            new User { Id = 1, FullName = "Mehrnaz Mehrnaz", Email = "admin@filmify.com" },
            new User { Id = 2, FullName = "Sahar Ahmadi", Email = "user@filmify.com" }
        );
    }
}