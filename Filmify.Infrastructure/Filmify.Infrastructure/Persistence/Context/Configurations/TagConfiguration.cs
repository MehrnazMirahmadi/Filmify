using Filmify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Filmify.Infrastructure.Persistence.Context.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(t => t.TagId);

        builder.Property(t => t.TagText)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(t => t.RegDate)
               .HasDefaultValueSql("GETUTCDATE()");

        builder.HasMany(t => t.FilmTags)
               .WithOne(ft => ft.Tag)
               .HasForeignKey(ft => ft.TagId);
    }
}