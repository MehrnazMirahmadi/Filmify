using Filmify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Filmify.Infrastructure.Persistence.Context.Configurations;

public class BoxConfiguration : IEntityTypeConfiguration<Box>
{
    public void Configure(EntityTypeBuilder<Box> builder)
    {
        builder.HasKey(b => b.BoxId);

        builder.Property(b => b.BoxName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(b => b.Slug)
               .IsRequired()
               .HasMaxLength(150);

        builder.HasMany(b => b.FilmBoxes)
               .WithOne(fb => fb.Box)
               .HasForeignKey(fb => fb.BoxId);
    }
}
