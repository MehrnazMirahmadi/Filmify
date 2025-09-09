namespace Filmify.Domain.Entities;

public class Box
{
    public int BoxID { get; set; }
    public string BoxName { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public int SortOrder { get; set; } = 0;

    
    public ICollection<FilmBox> FilmBoxes { get; set; } = new List<FilmBox>();
}
