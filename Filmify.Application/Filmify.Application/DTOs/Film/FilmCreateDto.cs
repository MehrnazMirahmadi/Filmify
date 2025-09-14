using Filmify.Domain.ValueObjects;

namespace Filmify.Application.DTOs.Film;

public class FilmCreateDto
{
    public string FilmTitle { get; set; } = null!;
    public Duration? Duration { get; set; }
    public string? CoverImage { get; set; }
    public long? Capacity { get; set; }
    public string? FileUrl { get; set; }
    public Guid RegisteringUserID { get; set; }
    public ICollection<long>? BoxIds { get; set; }
    public ICollection<long>? TagIds { get; set; }
}
