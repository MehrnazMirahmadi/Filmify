namespace Filmify.Application.DTOs.Box;

public class BoxDto
{
    public long BoxId { get; set; }
    public string BoxName { get; set; } = null!;
    public string Slug { get; set; } = null!;
}