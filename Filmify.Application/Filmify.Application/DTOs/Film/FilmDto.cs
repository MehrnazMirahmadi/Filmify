using Filmify.Domain.ValueObjects;

namespace Filmify.Application.DTOs.Film;

public class FilmDto
{
    public long FilmId{ get; set; }
    public string FilmTitle { get; set; } = null!;
    public Duration? Duration { get; set; } // map Duration.ValueAsString 
    public string? CoverImage { get; set; }
    public long? Capacity { get; set; }
    public DateTime RegDate { get; set; }
    public string? FileUrl { get; set; }
    public int LikeCount { get; set; }
    public int ViewCount { get; set; }
    public string? JsonLD { get; set; }
    public Guid RegisteringUserID { get; set; }
    public Guid? ApprovalUserID { get; set; }
    public DateTime? ApprovalDate { get; set; }

    public ICollection<string>? Tags { get; set; } 
    public ICollection<string>? Boxes { get; set; } 
}

