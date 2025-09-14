using Filmify.Application.Common.Paging;

namespace Filmify.Application.DTOs.Film;

public class FilmFilterRequest : KeysetPagingRequest
{
    public string? FilmTitle { get; set; }
    public string? BoxName { get; set; }
    public string? TagName { get; set; }
    public DateTime? FromRegDate { get; set; }
    public DateTime? ToRegDate { get; set; }
    public Guid? RegisteringUserID { get; set; }
    public bool? Approved { get; set; } // ApprovalUserID != null
}

