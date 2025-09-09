namespace Filmify.Domain.Entities;

public class FilmTag
{
    public int FilmID { get; set; }
    public int TagID { get; set; }
    public Film Film { get; set; } = null!;
    public Tag Tag { get; set; } = null!;
}