using Filmify.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Filmify.Infrastructure.Persistence.Seed;

public static class DbInitializer
{
    public static async Task SeedAsync(FilmifyDbContext context)
    {
        // --- Boxes ---
        if (!await context.Boxes.AnyAsync())
        {
            var boxes = SeedData.GetBoxes();
            await context.Boxes.AddRangeAsync(boxes);
            await context.SaveChangesAsync();
        }

        // --- Tags ---
        if (!await context.Tags.AnyAsync())
        {
            var tags = SeedData.GetTags();
            await context.Tags.AddRangeAsync(tags);
            await context.SaveChangesAsync();
        }

        // --- Films ---
        if (!await context.Films.AnyAsync())
        {
            var films = SeedData.GetFilms(await context.Boxes.ToListAsync(), await context.Tags.ToListAsync());
            await context.Films.AddRangeAsync(films);
            await context.SaveChangesAsync();
        }

        // --- FilmBoxes ---
        if (!await context.FilmBoxes.AnyAsync())
        {
            var filmBoxes = context.Films
                .SelectMany(f => f.FilmBoxes)
                .ToList();
            await context.FilmBoxes.AddRangeAsync(filmBoxes);
            await context.SaveChangesAsync();
        }

        // --- FilmTags ---
        if (!await context.FilmTags.AnyAsync())
        {
            var filmTags = context.Films
                .SelectMany(f => f.FilmTags)
                .ToList();
            await context.FilmTags.AddRangeAsync(filmTags);
            await context.SaveChangesAsync();
        }
    }

}
