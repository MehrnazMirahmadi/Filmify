using Filmify.Domain.Common;
using Filmify.Domain.ValueObjects;

namespace Filmify.Domain.Entities;

public class Film : BaseEntity
{
    public long FilmId { get; set; }

    public string FilmTitle { get; set; } = null!;
    public Duration? Duration { get; set; }

    public string? CoverImage { get; set; }          
    public long? Capacity { get; set; }              
    public DateTime RegDate { get; set; } = DateTime.UtcNow;

    public string? FileUrl { get; set; }
    public int LikeCount { get; set; } = 0;
    public int ViewCount { get; set; } = 0;

    public string? JsonLD { get; set; }              

    public Guid RegisteringUserID { get; set; }      
    public Guid? ApprovalUserID { get; set; }        
    public DateTime? ApprovalDate { get; set; }     

    // 
    public ICollection<FilmBox> FilmBoxes { get; set; } = new List<FilmBox>();
    public ICollection<FilmTag> FilmTags { get; set; } = new List<FilmTag>();
}
