namespace Filmify.Domain.Entities;

public class FilmBox
{
    public long FilmId { get; set; }
    public long BoxId { get; set; }
    public int SortOrder { get; set; } = 0;


    public Film Film { get; set; } = null!;
    public Box Box { get; set; } = null!;
}