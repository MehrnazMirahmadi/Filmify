using Filmify.Domain.Entities;
using Filmify.Domain.ValueObjects;

namespace Filmify.Infrastructure.Persistence.Seed;

public static class SeedData
{
    public static List<Category> GetCategories() => new()
    {
        new Category { Name = "Classic" },
        new Category { Name = "Action" },
        new Category { Name = "Comedy" },
        new Category { Name = "Drama" },
        new Category { Name = "Romance" },
        new Category { Name = "Horror" }
    };

    public static List<Box> GetBoxes() => new()
    {
        new Box { BoxName = "Action", Slug = "action", SortOrder = 1 },
        new Box { BoxName = "Comedy", Slug = "comedy", SortOrder = 2 },
        new Box { BoxName = "Drama", Slug = "drama", SortOrder = 3 },
        new Box { BoxName = "Romance", Slug = "romance", SortOrder = 4 },
        new Box { BoxName = "Horror", Slug = "horror", SortOrder = 5 }
    };

    public static List<Tag> GetTags()
    {
        var userId = Guid.NewGuid();
        return new List<Tag>
        {
            new Tag { TagText = "Popular", RegisteringUserID = userId },
            new Tag { TagText = "New Release", RegisteringUserID = userId },
            new Tag { TagText = "Classic", RegisteringUserID = userId },
            new Tag { TagText = "Drama", RegisteringUserID = userId },
            new Tag { TagText = "Comedy", RegisteringUserID = userId },
            new Tag { TagText = "Action", RegisteringUserID = userId },
            new Tag { TagText = "Romance", RegisteringUserID = userId },
            new Tag { TagText = "Horror", RegisteringUserID = userId }
        };
    }

    public static List<Film> GetFilms(List<Box> boxes, List<Tag> tags, List<Category> categories)
    {
        var films = new List<Film>();

        films.Add(CreateFilm("The First Adventure", "cover1.jpg", "film1.mp4", categories[1], 7,
            boxes: new[] { boxes[0], boxes[1] }, tags: new[] { tags[0], tags[5] }));

        films.Add(CreateFilm("Funny Moments", "cover2.jpg", "film2.mp4", categories[2], 8,
            boxes: new[] { boxes[1] }, tags: new[] { tags[1], tags[4] }));

        films.Add(CreateFilm("God Father", "cover3.jpg", "film3.mp4", categories[3], 9,
            boxes: new[] { boxes[2] }, tags: new[] { tags[2], tags[3] }));

        films.Add(CreateFilm("Romantic Nights", "cover4.jpg", "film4.mp4", categories[4], 7,
            boxes: new[] { boxes[3] }, tags: new[] { tags[0], tags[6] }));

        films.Add(CreateFilm("Scary Tales", "cover5.jpg", "film5.mp4", categories[5], 6,
            boxes: new[] { boxes[4] }, tags: new[] { tags[0], tags[7] }));

        films.Add(CreateFilm("Classic Journey", "cover6.jpg", "film6.mp4", categories[0], 8,
            boxes: new[] { boxes[0], boxes[2] }, tags: new[] { tags[2], tags[3] }));

        films.Add(CreateFilm("Action Blast", "cover7.jpg", "film7.mp4", categories[1], 9,
            boxes: new[] { boxes[0] }, tags: new[] { tags[5] }));

        films.Add(CreateFilm("Laugh Out Loud", "cover8.jpg", "film8.mp4", categories[2], 7,
            boxes: new[] { boxes[1] }, tags: new[] { tags[4] }));

        films.Add(CreateFilm("Drama King", "cover9.jpg", "film9.mp4", categories[3], 8,
            boxes: new[] { boxes[2] }, tags: new[] { tags[3] }));

        films.Add(CreateFilm("Love in Paris", "cover10.jpg", "film10.mp4", categories[4], 7,
            boxes: new[] { boxes[3] }, tags: new[] { tags[6] }));

        films.Add(CreateFilm("Horror Nights", "cover11.jpg", "film11.mp4", categories[5], 6,
            boxes: new[] { boxes[4] }, tags: new[] { tags[7] }));

        films.Add(CreateFilm("Epic Saga", "cover12.jpg", "film12.mp4", categories[0], 9,
            boxes: new[] { boxes[0], boxes[2] }, tags: new[] { tags[2] }));

        films.Add(CreateFilm("Adventure Time", "cover13.jpg", "film13.mp4", categories[1], 8,
            boxes: new[] { boxes[0] }, tags: new[] { tags[0], tags[5] }));

        films.Add(CreateFilm("Comedy Stars", "cover14.jpg", "film14.mp4", categories[2], 7,
            boxes: new[] { boxes[1] }, tags: new[] { tags[1], tags[4] }));

        films.Add(CreateFilm("Romantic Drama", "cover15.jpg", "film15.mp4", categories[4], 8,
            boxes: new[] { boxes[3] }, tags: new[] { tags[3], tags[6] }));

        return films;
    }

    private static Film CreateFilm(string title, string cover, string file, Category category, int score, Box[] boxes, Tag[] tags)
    {
        var film = new Film
        {
            FilmTitle = title,
            Duration = new Duration(90, 0),
            CoverImage = cover,
            FileUrl = file,
            RegisteringUserID = Guid.NewGuid(),
            Category = category,
            FilmScore = score
        };

        foreach (var box in boxes)
        {
            film.FilmBoxes.Add(new FilmBox { Film = film, Box = box, SortOrder = box.SortOrder });
        }

        foreach (var tag in tags)
        {
            film.FilmTags.Add(new FilmTag { Film = film, Tag = tag });
        }

        return film;
    }
}
