namespace Filmify.Domain.Entities;

public class FilmTag
{
    public long FilmId { get; set; }
    public long TagId { get; set; }
    public Film Film { get; set; } = null!;
    public Tag Tag { get; set; } = null!;
}