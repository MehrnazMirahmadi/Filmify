using Filmify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Filmify.Infrastructure.Persistence.Context.Configurations;


public class FilmTagConfiguration : IEntityTypeConfiguration<FilmTag>
{
    public void Configure(EntityTypeBuilder<FilmTag> builder)
    {
        builder.HasKey(ft => new { ft.FilmId, ft.TagId });

        builder.HasOne(ft => ft.Film)
               .WithMany(f => f.FilmTags)
               .HasForeignKey(ft => ft.FilmId);

        builder.HasOne(ft => ft.Tag)
               .WithMany(t => t.FilmTags)
               .HasForeignKey(ft => ft.TagId);
    }
}
