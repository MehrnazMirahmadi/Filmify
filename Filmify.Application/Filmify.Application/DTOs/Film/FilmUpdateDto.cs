using Filmify.Application.DTOs.Tag;
using Filmify.Domain.ValueObjects;

namespace Filmify.Application.DTOs.Film;

public class FilmUpdateDto
{
    public long FilmId { get; set; }
    public string FilmTitle { get; set; } = null!;
    public Duration? Duration { get; set; }
    public string? CoverImage { get; set; }
    public long? Capacity { get; set; }
    public string? FileUrl { get; set; }
    public ICollection<long>? BoxIds { get; set; }
    public ICollection<long>? TagIds { get; set; }
    public List<TagDto> AllTags { get; set; } = new List<TagDto>();
}