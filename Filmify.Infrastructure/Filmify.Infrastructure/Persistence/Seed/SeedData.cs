using Filmify.Domain.Entities;
using Filmify.Domain.ValueObjects;

namespace Filmify.Infrastructure.Persistence.Seed;

public static class SeedData
{
    public static List<Category> GetCategories()
    {
              return new List<Category>
          {
              new Category { Name = "Classic" },
              new Category { Name = "Action" },
              new Category { Name = "Comedy" },
              new Category { Name = "Drama" }
          };
    }

    public static List<Box> GetBoxes()
    {
        return new List<Box>
        {
            new Box { BoxName = "Action", Slug = "action", SortOrder = 1 },
            new Box { BoxName = "Comedy", Slug = "comedy", SortOrder = 2 },
            new Box { BoxName = "Drama", Slug = "drama", SortOrder = 3 },
        };
    }

    public static List<Tag> GetTags()
    {
        return new List<Tag>
        {
            new Tag { TagText = "Popular", RegisteringUserID = Guid.NewGuid() },
            new Tag { TagText = "New Release", RegisteringUserID = Guid.NewGuid() },
            new Tag { TagText = "Classic", RegisteringUserID = Guid.NewGuid() },
            new Tag { TagText = "Drama", RegisteringUserID = Guid.NewGuid() },
        };
    }

    public static List<Film> GetFilms(List<Box> boxes, List<Tag> tags, List<Category> categories)
    {
        var film1 = new Film
        {
            FilmTitle = "The First Adventure",
            Duration = new Duration(120, 0),
            CoverImage = "cover.jpg",
            FileUrl = "film1.mp4",
            RegisteringUserID = Guid.NewGuid(),
            Category = categories.First(c => c.Name == "Action"),
            FilmScore = 7

        };

        // Attach Boxes
        film1.FilmBoxes.Add(new FilmBox { Film = film1, Box = boxes[0], SortOrder = 1 });
        film1.FilmBoxes.Add(new FilmBox { Film = film1, Box = boxes[1], SortOrder = 2 });

        // Attach Tags
        film1.FilmTags.Add(new FilmTag { Film = film1, Tag = tags[0] });
        film1.FilmTags.Add(new FilmTag { Film = film1, Tag = tags[1] });

        var film2 = new Film
        {
            FilmTitle = "Funny Moments",
            Duration = new Duration(90, 30),
            CoverImage = "cover2.jpg",
            FileUrl = "film2.mp4",
            RegisteringUserID = Guid.NewGuid(),
            Category = categories.First(c => c.Name == "Comedy"),
            FilmScore = 8
        };


        film2.FilmBoxes.Add(new FilmBox { Film = film2, Box = boxes[1], SortOrder = 1 });
        film2.FilmTags.Add(new FilmTag { Film = film2, Tag = tags[1] });
        film2.FilmTags.Add(new FilmTag { Film = film2, Tag = tags[2] });


        var film3 = new Film
        {
            FilmTitle = "God father",
            Duration = new Duration(90, 30),
            CoverImage = "cover3.jpg",
            FileUrl = "film2.mp4",
            RegisteringUserID = Guid.NewGuid(),
            Category = categories.First(c => c.Name == "Drama"),
            FilmScore = 9
        };


        film2.FilmBoxes.Add(new FilmBox { Film = film2, Box = boxes[1], SortOrder = 1 });
        film2.FilmTags.Add(new FilmTag { Film = film2, Tag = tags[1] });
        film2.FilmTags.Add(new FilmTag { Film = film2, Tag = tags[2] });

        return new List<Film> { film1, film2 };
    }
}
