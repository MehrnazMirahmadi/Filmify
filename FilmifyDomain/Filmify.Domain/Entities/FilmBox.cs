namespace Filmify.Domain.Entities;

public class FilmBox
{
    public int FilmID { get; set; }
    public int BoxID { get; set; }
    public int SortOrder { get; set; } = 0;

    // روابط
    public Film Film { get; set; } = null!;
    public Box Box { get; set; } = null!;
}