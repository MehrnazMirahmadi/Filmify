using Filmify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Filmify.Infrastructure.Persistence.Context.Configurations;

public class FilmBoxConfiguration : IEntityTypeConfiguration<FilmBox>
{
    public void Configure(EntityTypeBuilder<FilmBox> builder)
    {
        builder.HasKey(fb => new { fb.FilmId, fb.BoxId });

        builder.HasOne(fb => fb.Film)
               .WithMany(f => f.FilmBoxes)
               .HasForeignKey(fb => fb.FilmId);

        builder.HasOne(fb => fb.Box)
               .WithMany(b => b.FilmBoxes)
               .HasForeignKey(fb => fb.BoxId);
    }
}