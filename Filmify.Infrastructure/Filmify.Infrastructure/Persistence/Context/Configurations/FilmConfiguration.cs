using Filmify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Filmify.Infrastructure.Persistence.Context.Configurations;

public class FilmConfiguration : IEntityTypeConfiguration<Film>
{
    public void Configure(EntityTypeBuilder<Film> builder)
    {
        builder.HasKey(f => f.FilmId);

        builder.Property(f => f.FilmTitle)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(f => f.CoverImage)
               .HasMaxLength(500);

        builder.Property(f => f.FileUrl)
               .HasMaxLength(500);

        builder.Property(f => f.RegDate)
               .HasDefaultValueSql("GETUTCDATE()");

        builder.HasMany(f => f.FilmBoxes)
               .WithOne(fb => fb.Film)
               .HasForeignKey(fb => fb.FilmId);

        builder.HasMany(f => f.FilmTags)
               .WithOne(ft => ft.Film)
               .HasForeignKey(ft => ft.FilmId);
        // ----- Duration as Owned ValueObject -----
        builder.OwnsOne(f => f.Duration, duration =>
        {
            duration.Property(d => d.Minutes)
                .HasColumnName("DurationMinutes")
                .IsRequired();

            duration.Property(d => d.Seconds)
                .HasColumnName("DurationSeconds")
                .IsRequired();
        });
    }
}